using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

// Con GetMyGoalPosition() Podemos saber de que lado de la porteria estamos

namespace Teams.ORTcoder
{
    public class PlayerOne : TeamPlayer
    {   
        const int DEFENSIVO = 0;
        const int OFENSIVO = 1;
        int modo = DEFENSIVO;

        const float MAX_DIST_DEL_ARCO = 3.2f;
        const float MIN_DIST_DEL_ARCO = 2.7f;
        const float MAX_DIST_ARCO_PELOTA = 6f;

        public override void OnUpdate()
        {
            var PlayerOMasCercano = ClosestOPlayerToBall(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetTeamMatesInformation()[0].Position, GetTeamMatesInformation()[1].Position, GetBallPosition());
            var DistanciaBola = Vector3.Distance(GetPosition(), GetBallPosition());

            var PlayerOMasCercano2 = ClosestOPlayerToArco(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetMyGoalPosition());
            var DistanciaArco = Vector3.Distance(GetPosition(), GetMyGoalPosition());

            if (PlayerOMasCercano > DistanciaBola && PlayerOMasCercano2 > DistanciaArco - 0.5f){
                modo = OFENSIVO;
            }
            else{
                modo = DEFENSIVO;
            }

            switch (modo)
            {
                case DEFENSIVO:
                    var posicionPelota = GetBallPosition();
                    var posicionMiArco = GetMyGoalPosition();
                    var posicionMia = GetPosition();
                    var puntoMasCercano = ClosestPointOnLineAP(posicionPelota, posicionMiArco, posicionMia);
                    GoTo(puntoMasCercano);
                    break;
                case OFENSIVO:
                    GoTo(GetBallPosition());
                    break;
            }
            Debug.DrawLine(GetBallPosition(), GetMyGoalPosition(), Color.magenta, 0.2f);
        }

        public override void OnReachBall()
        {
            if (modo == DEFENSIVO){
                modo = OFENSIVO;
                var mejorPasee = mejorPase(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetTeamMatesInformation()[0].Position, GetTeamMatesInformation()[1].Position, GetPosition());
                var directionPase = GetDirectionTo(mejorPasee);
                ShootBall(directionPase, ShootForce.High);
                modo = DEFENSIVO;
            }
            else{
                var mejorPasee = mejorPase(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetTeamMatesInformation()[0].Position, GetTeamMatesInformation()[1].Position, GetPosition());
                var directionPase = GetDirectionTo(mejorPasee);
                ShootBall(directionPase, ShootForce.High);
            }
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
        
        }

        public override FieldPosition GetInitialPosition() => FieldPosition.A2;

        public override string GetPlayerDisplayName() => "Kippur";

        // Esta funcion telleva al punto mas cercano de una linea pero esta medio modificada
        public static Vector3 ClosestPointOnLineAP(Vector3 vA, Vector3 vB, Vector3 vPoint)
        {
            var vVector1 = vPoint - vA;
            var vVector2 = (vB - vA).normalized;
        
            var d = Vector3.Distance(vA, vB);
            var d2 = Vector3.Distance(vB, vPoint);
            var t = Vector3.Dot(vVector2, vVector1);

            if ((t <= 0 || d2 <= MIN_DIST_DEL_ARCO) && d > MAX_DIST_ARCO_PELOTA){
                return vA;
            }
            if (t >= d || d2 >= MAX_DIST_DEL_ARCO){
                return vB;
            }

            var vVector3 = vVector2 * t;
            var vClosestPoint = vA + vVector3;
        
            return vClosestPoint;
        }

        public static Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
        {
            var vVector1 = vPoint - vA;
            var vVector2 = (vB - vA).normalized;
        
            var d = Vector3.Distance(vA, vB);
            var t = Vector3.Dot(vVector2, vVector1);

            if (t <= 0.0f){
                return vA;
            }
            if (t >= d){
                return vB;
            }

            var vVector3 = vVector2 * t;
            var vClosestPoint = vA + vVector3;
        
            return vClosestPoint;
        }

        public static Vector3 mejorPase(Vector3 vO1, Vector3 vO2, Vector3 vO3, Vector3 vF1, Vector3 vF2, Vector3 vMe){
            Vector3[] positionOArray = new []{vO1, vO2, vO3};
            Vector3[] positionFArray = new []{vF1, vF2};

            var masLejano = 0.0f;
            var compa = 0;

            for (int p = 0; p < 3; p++){
                var ClosestPoint = ClosestPointOnLine(vMe, positionOArray[p], positionFArray[0]);
                var x = Vector3.Distance(ClosestPoint, positionOArray[p]);
                if (x > masLejano){
                    compa = 0;
                }
            }
            
            for (int p = 0; p < 3; p++){
                var ClosestPoint = ClosestPointOnLine(vMe, positionOArray[p], positionFArray[1]);
                var x = Vector3.Distance(ClosestPoint, positionOArray[p]);
                if (x > masLejano){
                    compa = 1;
                }
            }

            return positionFArray[compa];
        }

        // El Jugador mas cercano a la bolA 
        public static float ClosestOPlayerToBall(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e, Vector3 vBall){
            Vector3[] positionArray = new []{a, b, c, d, e};
            
            var masCercano = 99999.0f;

            for (int i = 0; i < 3; i++){
                var x = Vector3.Distance(positionArray[i], vBall);
                if (x < masCercano){
                    masCercano = x;
                }
            }
            return masCercano;
        }

        // El jugador mas cercano enemigo mas cercano al arco 
        public static float ClosestOPlayerToArco(Vector3 a, Vector3 b, Vector3 c, Vector3 vArco){
            var masCercano = 99999.0f;
            Vector3[] positionArray = new []{a, b, c };
            for (int i = 0; i < 3; i++){
                var x = Vector3.Distance(positionArray[i], vArco);
                if (x < masCercano){
                    masCercano = x;
                }
            }
            return masCercano;
        }
    }
}