using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;


namespace Teams.ORTcoderMate
{
    public class PlayerOne : TeamPlayer
    {
        public bool IsNearest()
        {
            float distance = Vector3.Distance(GetPosition(), GetBallPosition());
            float distance2 = Vector3.Distance(GetTeamMatesInformation()[0].Position, GetBallPosition());
            float distance3 = Vector3.Distance(GetTeamMatesInformation()[1].Position, GetBallPosition());
            float distance4 = Vector3.Distance(GetRivalsInformation()[0].Position, GetBallPosition());
            float distance5 = Vector3.Distance(GetRivalsInformation()[1].Position, GetBallPosition());
            return distance < distance2 && distance < distance3 && distance < distance4 && distance < distance5;
        }

        public Vector3 GetPos()
        {
            var x = GetMyGoalPosition()[0] > 0 ? 10 : -10;
            var ballPosition = GetBallPosition();
            Vector3 origin = new Vector3(x, 0.0f, 0.0f);
            var radius = 3;

            Vector3 originToBall = ballPosition - origin;
            Vector3 originToBallNorm = originToBall.normalized;
            Vector3 originToPos = radius * originToBallNorm;
            Vector3 posFinal = origin + originToPos;

            return posFinal;
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

        public override void OnUpdate()
        {
            if (IsNearest() && (GetMyGoalPosition()[0] < 0 && GetBallPosition()[0] < -6) || (GetMyGoalPosition()[0] > 0 && GetBallPosition()[0] > 6)) {
                GoTo(GetBallPosition());
            } else {
                MoveBy(GetDirectionTo(GetPos()));
            }
        }

        public override void OnReachBall()
        {
            if (CanShoot(GetTeamMatesInformation()[0].Position, 0.5f)) {
                Vector3 pos = GetTeamMatesInformation()[0].Position;
                ShootBall(GetDirectionTo(pos), GetForce(pos));
                Debug.Log("Goalie: Pase a Mid");
            } else if (CanShoot(GetTeamMatesInformation()[1].Position, 1.0f)) {
                Vector3 pos = GetTeamMatesInformation()[1].Position;
                ShootBall(GetDirectionTo(pos), GetForce(pos));
                Debug.Log("Goalie: Pase a Messi");
            } else if (CanShoot(GetRivalGoalPosition(), 1.0f)) {
                ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
                Debug.Log("Goalie: Tiro al Arco");
            } else {
                int x = GetMyGoalPosition()[0] > 0 ? -10 : 10;
                int z = GetBallPosition()[2] > 0 ? -6: 6;
                Vector3 newPos = new Vector3(x, 0, z);
                if (CanShoot(newPos, 2.0f)) {
                    ShootBall(GetDirectionTo(newPos), ShootForce.High);
                } else {
                    ShootBall(GetDirectionTo(new Vector3(-x, 0, z)), ShootForce.Medium);
                }
            }
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public override FieldPosition GetInitialPosition() => FieldPosition.A2;

        public override string GetPlayerDisplayName() => "Goalie";
    }
}