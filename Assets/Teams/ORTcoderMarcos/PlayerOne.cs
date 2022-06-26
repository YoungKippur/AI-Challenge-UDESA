using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

// Con GetMyGoalPosition() Podemos saber de que lado de la porteria estamos

namespace Teams.ORTcoderMarcos
{
    public class PlayerOne : TeamPlayer
    {   
        const int DEFENSIVO = 0;
        const int OFENSIVO = 1;
        int modo = DEFENSIVO;

        const float MAX_DIST_DEL_ARCO = 4f;
        const float MIN_DIST_DEL_ARCO = 2.5f;
        const float MAX_DIST_ARCO_PELOTA = 7f;

        public override void OnUpdate()
        {
            var PlayerMasCercano = ClosestOPlayerToBall(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetBallPosition());
            var DistanciaBola = Vector3.Distance(GetPosition(), GetBallPosition());

            var PlayerMasCercano2 = ClosestOPlayerToArco(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetMyGoalPosition());
            var DistanciaArco = Vector3.Distance(GetPosition(), GetMyGoalPosition());

            if (PlayerMasCercano > DistanciaBola && PlayerMasCercano2 > DistanciaArco - 0.5f){
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
                // modo = OFENSIVO;
                // var playerTwoPosition = GetTeamMatesInformation()[1].Position;
                // var directionToPlayerTwo = GetDirectionTo(playerTwoPosition);
                // ShootBall(directionToPlayerTwo, ShootForce.High);
                // modo = DEFENSIVO;

                modo = OFENSIVO;
                var directionToArco = GetDirectionTo(GetRivalGoalPosition());
                ShootBall(directionToArco, ShootForce.High);
                modo = DEFENSIVO;
            }
            else{
                // var playerTwoPosition = GetTeamMatesInformation()[1].Position;
                // var directionToPlayerTwo = GetDirectionTo(playerTwoPosition);
                // ShootBall(directionToPlayerTwo, ShootForce.High);

                var directionToArco = GetDirectionTo(GetRivalGoalPosition());
                ShootBall(directionToArco, ShootForce.High);
            }
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
            
        }

        public override FieldPosition GetInitialPosition() => FieldPosition.A2;

        public override string GetPlayerDisplayName() => "Kippur";

        // Esta funcion telleva al punto mas cercano de una linea  
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

        public static float ClosestOPlayerToBall(Vector3 a, Vector3 b, Vector3 c, Vector3 vBall){
            var masCercano = 99999.0f;
            Vector3[] positionArray = new []{a, b, c };
            for (int i = 0; i < 3; i++){
                var x = Vector3.Distance(positionArray[i], vBall);
                if (x < masCercano){
                    masCercano = x;
                }
            }
            return masCercano;
        }

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