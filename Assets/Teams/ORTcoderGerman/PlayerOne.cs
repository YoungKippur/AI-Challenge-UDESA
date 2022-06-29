using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ORTcoderGerman
{
    public class PlayerOne : TeamPlayer
    {
        public override void OnUpdate()
        {
            if(IsLossing())
            {

            }
            else if (IsWinning())
            {

            }
            {
                if (IsNearest())
                {
                    GoTo(GetBallPosition());
                }
                else if (Vector3.Distance(GetBallPosition(), GetMyGoalPosition()) < 6.0f)
                {
                    float x = GetMyGoalPosition()[0] > 0.0f ? 6.0f : -6.0f;
                    float z = GetBallPosition()[2] > 0 ? -4.0f : 4.0f;
                    GoTo(new Vector3(x, 0, z));
                }
                else if (Vector3.Distance(GetBallPosition(), GetMyGoalPosition()) < 14.0f)
                {
                    GoTo(GetBallPosition());
                }
                else
                {
                    int x = GetMyGoalPosition()[0] > 0 ? -4 : 4;
                    int z = GetBallPosition()[2] > 0 ? -4 : 4;
                    GoTo(new Vector3(x, 0, z));
                }
            }
        }

        public bool IsNearest()
        {
            float distance = Vector3.Distance(GetPosition(), GetBallPosition());
            float distance2 = Vector3.Distance(GetTeamMatesInformation()[0].Position, GetBallPosition());
            float distance3 = Vector3.Distance(GetTeamMatesInformation()[1].Position, GetBallPosition());
            float distance4 = Vector3.Distance(GetRivalsInformation()[0].Position, GetBallPosition());
            float distance5 = Vector3.Distance(GetRivalsInformation()[1].Position, GetBallPosition());
            return distance < distance2 && distance < distance3 && distance < distance4 && distance < distance5;
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

        public override void OnReachBall()
        {
            if(IsLossing() && GetTimeLeft() <= 3)
            {
                ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
                Debug.Log("Messi: Tiro al Arco");
            }
            else if (CanShoot(GetRivalGoalPosition(), 0.3f))
            {
                ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
                Debug.Log("Messi: Tiro al Arco");
            }
            else if (CanShoot(GetTeamMatesInformation()[1].Position, 0.3f))
            {
                Vector3 pos = GetTeamMatesInformation()[1].Position;
                ShootBall(GetDirectionTo(pos), GetForce(pos));
                Debug.Log("Messi: Pase a Di Maria");
            }
            else
            {
                Vector3 pos = GetTeamMatesInformation()[0].Position;
                ShootBall(GetDirectionTo(pos), GetForce(pos));
                Debug.Log("Messi: Pase a Caballero");
            }
        }

        public bool IsLossing()
        {
            return GetMyScore() < GetRivalScore();
        }
        public bool IsWinning()
        {
            return GetMyScore() > GetRivalScore();
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public override FieldPosition GetInitialPosition() => FieldPosition.C1;

        public override string GetPlayerDisplayName() => "Messi";
    }
}