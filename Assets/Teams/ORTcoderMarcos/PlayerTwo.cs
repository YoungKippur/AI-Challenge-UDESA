using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ORTcoderMarcos
{
    public class PlayerTwo : TeamPlayer
    {
        const int DEFENSIVO = 0;
        const int OFENSIVO = 1;
        int modo = DEFENSIVO;

        const float MAX_DIST_DEL_ARCO = 1f;
        const float MIN_DIST_DEL_ARCO = 1f;

        public override void OnUpdate()
        {
            switch (modo)
            {
                case DEFENSIVO:
                    var posicionEnemigo1 = GetRivalsInformation()[1].Position;
                    var posicionEnemigo2 = GetRivalsInformation()[2].Position;
                    var posicionMia = GetPosition();
                    var marca = ClosestPointOnLineaa(posicionEnemigo1, posicionEnemigo2, posicionMia, GetBallPosition());
                    GoTo(marca);
                    var marca2 = ClosestPointOnLineaa(GetMyGoalPosition(), posicionEnemigo2, posicionMia, GetBallPosition());
                    if (Vector3.Distance(posicionMia, GetBallPosition()) < 4.5f){
                        modo = OFENSIVO;
                    }
                    else if (Vector3.Distance(posicionEnemigo1, posicionEnemigo2) > 8f){
                        GoTo(marca2);
                    }
                    break;
                case OFENSIVO:
                    GoTo(GetBallPosition());
                    if (Vector3.Distance(GetPosition(), GetBallPosition()) >= 4.5f){
                        modo = DEFENSIVO;
                    }
                    break;
            }
        }

        public override void OnReachBall()
        {

            var mejorPasee = mejorPase(GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetRivalGoalPosition(), GetTeamMatesInformation()[1].Position, GetPosition(), GetRivalGoalPosition());
	        var mejorPaseDirection = GetDirectionTo(mejorPasee);
	        ShootBall(mejorPaseDirection, ShootForce.High);
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public override FieldPosition GetInitialPosition() => FieldPosition.B2;

        public override string GetPlayerDisplayName() => "El Marcas";

        public static Vector3 ClosestPointOnLineaa(Vector3 vA, Vector3 vB, Vector3 vPoint, Vector3 vBall)
        {
            var vVector1 = vPoint - vA;
            var vVector2 = (vB - vA).normalized;
        
            var d = Vector3.Distance(vA, vB);
            var d2 = Vector3.Distance(vB, vPoint);
            var d3 = Vector3.Distance(vA, vPoint);
            var t = Vector3.Dot(vVector2, vVector1);

            if ((t <= 0 || (d2 <= MIN_DIST_DEL_ARCO && d3 > MAX_DIST_DEL_ARCO))){
                return vA;
            }
            if (t >= d || (d3 <= MAX_DIST_DEL_ARCO && d2 > MIN_DIST_DEL_ARCO)){
                return vB;
            }
            if(t >= d || (d3 <= MAX_DIST_DEL_ARCO && d2 <= MIN_DIST_DEL_ARCO )){
                return vBall;
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

        public static Vector3 mejorPase(Vector3 vO1, Vector3 vO2, Vector3 vF1, Vector3 vF2, Vector3 vMe, Vector3 vArco){
            Vector3[] positionOArray = new []{vO1, vO2, vArco};
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
    }
}