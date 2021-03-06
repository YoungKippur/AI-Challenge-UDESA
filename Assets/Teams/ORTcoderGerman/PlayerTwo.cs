using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ORTcoderGerman
{
    public class PlayerTwo : TeamPlayer
    {
        float espera = 0;
        const int DEFENSIVO = 0;
        const int OFENSIVO = 1;
        int modo = DEFENSIVO;

        const float MAX_DIST_DEL_ARCO = 3.2f;
        const float MIN_DIST_DEL_ARCO = 2.7f;
        const float MAX_DIST_ARCO_PELOTA = 6f;

        public override void OnUpdate()
        {
            if (IsLossing() && GetTimeLeft() <= 15 && GetTimeLeft() <= espera)
            {
                if (IsNearest())
                {
                    GoTo(GetBallPosition());
                }
                else if (Vector3.Distance(GetBallPosition(), GetMyGoalPosition()) < 6.0f)
                {
                    GoTo(GetBallPosition());
                }
                else
                {
                    var PlayerOMasCercano = ClosestOPlayerToBall(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetTeamMatesInformation()[0].Position, GetTeamMatesInformation()[1].Position, GetBallPosition());
                    var DistanciaBola = Vector3.Distance(GetPosition(), GetBallPosition());

                    var PlayerOMasCercano2 = ClosestOPlayerToArco(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetMyGoalPosition());

                    var DistanciaArco = Vector3.Distance(GetPosition(), GetMyGoalPosition());

                    if (PlayerOMasCercano > DistanciaBola && PlayerOMasCercano2 > DistanciaArco - 0.5f)
                    {
                        modo = OFENSIVO;
                    }
                    else
                    {
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
            }
            else
            {
                var PlayerOMasCercano = ClosestOPlayerToBall(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetTeamMatesInformation()[0].Position, GetTeamMatesInformation()[1].Position, GetBallPosition());
                var DistanciaBola = Vector3.Distance(GetPosition(), GetBallPosition());

                var PlayerOMasCercano2 = ClosestOPlayerToArco(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetMyGoalPosition());

                var DistanciaArco = Vector3.Distance(GetPosition(), GetMyGoalPosition());

                if (PlayerOMasCercano > DistanciaBola && PlayerOMasCercano2 > DistanciaArco - 0.5f)
                {
                    modo = OFENSIVO;
                }
                else
                {
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
        }

         public override void OnReachBall()
            {
            if (IsLossing() && GetTimeLeft() <= 15)
            {
                if(GetTimeLeft() <= 3)
                {
                    ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
                    Debug.Log("Caballero: Tiro al Arco");
                }
                else if (CanShoot(GetRivalGoalPosition(), 0.3f))
                {
                    ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
                    Debug.Log("Caballero: Tiro al Arco");
                }
                else if (CanShoot(GetTeamMatesInformation()[1].Position, 0.3f))
                {
                    Vector3 pos = GetTeamMatesInformation()[1].Position;
                    ShootBall(GetDirectionTo(pos), GetForce(pos));
                    Debug.Log("Caballero: Pase a Maradona");
                }
                else
                {
                    Vector3 pos = GetTeamMatesInformation()[0].Position;
                    ShootBall(GetDirectionTo(pos), GetForce(pos));
                    Debug.Log("Caballero: Pase a Di Maria");
                }
            }
            else if (IsWinning())
            {
                if (CanShoot(GetTeamMatesInformation()[0].Position, 0.3f))
                {
                    Vector3 pos = GetTeamMatesInformation()[0].Position;
                    ShootBall(GetDirectionTo(pos), GetForce(pos));
                    Debug.Log("Caballero: Pase a Di Maria");
                }
                else if (CanShoot(GetRivalGoalPosition(), 0.1f))
                {
                    ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
                    Debug.Log("Caballero: Tiro al Arco");
                }
                else
                {
                    Vector3 pos = GetTeamMatesInformation()[1].Position;
                    ShootBall(GetDirectionTo(pos), GetForceW(pos));
                    Debug.Log("Caballero: Pase a Maradona");
                }
            }
            else
            {
                if (modo == DEFENSIVO)
                {

                    modo = OFENSIVO;
                    var mejorPasee = mejorPase(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetTeamMatesInformation()[0].Position, GetTeamMatesInformation()[1].Position, GetPosition());
                    var directionPase = GetDirectionTo(mejorPasee);
                    ShootBall(directionPase, ShootForce.High);
                    modo = DEFENSIVO;
                }
                else
                {
                    var mejorPasee = mejorPase(GetRivalsInformation()[0].Position, GetRivalsInformation()[1].Position, GetRivalsInformation()[2].Position, GetTeamMatesInformation()[0].Position, GetTeamMatesInformation()[1].Position, GetPosition());
                    var directionPase = GetDirectionTo(mejorPasee);
                    ShootBall(directionPase, ShootForce.High);
                }
            }
         }
         public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
         {
            float tLeft = GetTimeLeft();
            espera = tLeft - 2.5f;
        }

        public override FieldPosition GetInitialPosition() => FieldPosition.A2;

        public override string GetPlayerDisplayName() => "Caballero";

        public static Vector3 ClosestPointOnLineAP(Vector3 vA, Vector3 vB, Vector3 vPoint)
        {
            var vVector1 = vPoint - vA;
            var vVector2 = (vB - vA).normalized;

            var d = Vector3.Distance(vA, vB);
            var d2 = Vector3.Distance(vB, vPoint);
            var t = Vector3.Dot(vVector2, vVector1);

            if ((t <= 0 || d2 <= MIN_DIST_DEL_ARCO) && d > MAX_DIST_ARCO_PELOTA)
            {
                return vA;
            }
            if (t >= d || d2 >= MAX_DIST_DEL_ARCO)
            {
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

            if (t <= 0.0f)
            {
                return vA;
            }
            if (t >= d)
            {
                return vB;
            }

            var vVector3 = vVector2 * t;
            var vClosestPoint = vA + vVector3;

            return vClosestPoint;
        }

        public static Vector3 mejorPase(Vector3 vO1, Vector3 vO2, Vector3 vO3, Vector3 vF1, Vector3 vF2, Vector3 vMe)
        {
            Vector3[] positionOArray = new[] { vO1, vO2, vO3 };
            Vector3[] positionFArray = new[] { vF1, vF2 };

            var masLejano = 0.0f;
            var compa = 0;

            for (int p = 0; p < 3; p++)
            {
                var ClosestPoint = ClosestPointOnLine(vMe, positionOArray[p], positionFArray[0]);
                var x = Vector3.Distance(ClosestPoint, positionOArray[p]);
                if (x > masLejano)
                {
                    compa = 0;
                }
            }

            for (int p = 0; p < 3; p++)
            {
                var ClosestPoint = ClosestPointOnLine(vMe, positionOArray[p], positionFArray[1]);
                var x = Vector3.Distance(ClosestPoint, positionOArray[p]);
                if (x > masLejano)
                {
                    compa = 1;
                }
            }

            return positionFArray[compa];
        }

        public static float ClosestOPlayerToBall(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e, Vector3 vBall)
        {
            Vector3[] positionArray = new[] { a, b, c, d, e };

            var masCercano = 99999.0f;

            for (int i = 0; i < 3; i++)
            {
                var x = Vector3.Distance(positionArray[i], vBall);
                if (x < masCercano)
                {
                    masCercano = x;
                }
            }
            return masCercano;
        }

        public static float ClosestOPlayerToArco(Vector3 a, Vector3 b, Vector3 c, Vector3 vArco)
        {
            var masCercano = 99999.0f;
            Vector3[] positionArray = new[] { a, b, c };
            for (int i = 0; i < 3; i++)
            {
                var x = Vector3.Distance(positionArray[i], vArco);
                if (x < masCercano)
                {
                    masCercano = x;
                }
            }
            return masCercano;
        }

        public bool IsLossing()
        {
            return GetMyScore() < GetRivalScore();
        }
        public bool IsWinning()
        {
            return GetMyScore() > GetRivalScore();
        }

        public bool IsNearest()
        {
            float distance = Vector3.Distance(GetPosition(), GetBallPosition());
            float distance2 = Vector3.Distance(GetTeamMatesInformation()[0].Position, GetBallPosition());
            float distance3 = Vector3.Distance(GetTeamMatesInformation()[1].Position, GetBallPosition());
            float distance4 = Vector3.Distance(GetRivalsInformation()[0].Position, GetBallPosition());
            float distance5 = Vector3.Distance(GetRivalsInformation()[1].Position, GetBallPosition());
            float distance6 = Vector3.Distance(GetRivalsInformation()[2].Position, GetBallPosition());
            return distance < distance2 && distance < distance3 && distance < distance4 && distance < distance5 && distance < distance6;
        }

        public bool CanShoot(Vector3 position, float maxDistance)
        {
            var startingPoint = GetPosition();
            var direction = GetDirectionTo(position);
            foreach (var player in GetRivalsInformation())
            {
                Vector3 point = player.Position;
                Ray ray = new Ray(startingPoint, Vector3.Normalize(direction));
                float distance = Vector3.Cross(ray.direction, point - ray.origin).magnitude;
                if (distance < maxDistance) return false;
            }
            return true;
        }

        public Core.Games.ShootForce GetForce(Vector3 position)
        {
            float distance = Vector3.Distance(GetPosition(), position);
            if (distance < 4.0f) { return ShootForce.Low; }
            if (distance < 8.0f) { return ShootForce.Medium; }
            return ShootForce.High;
        }
        public Core.Games.ShootForce GetForceW(Vector3 position)
        {
            float distance = Vector3.Distance(GetPosition(), position);
            if (distance < 6.5f) { return ShootForce.Low; }
            return ShootForce.Medium;
        }
    }
}