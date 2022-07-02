using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ORTcoder
{
    public class PlayerThree : TeamPlayer
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

        public bool CanShoot(Vector3 position, float maxDistance)
        {
            var startingPoint = GetPosition();
            var direction = GetDirectionTo(position);
            foreach (var player in GetRivalsInformation()) {
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
            if (IsNearest() || Vector3.Distance(GetBallPosition(), GetMyGoalPosition()) > 12.0f) {
                GoTo(GetBallPosition());
            } else if (Vector3.Distance(GetBallPosition(), GetMyGoalPosition()) > 8.0f) {
                int x = GetMyGoalPosition()[0] > 0 ? -8 : 8;
                int z = GetBallPosition()[2] > 0 ? -6 : 6;
                GoTo(new Vector3(x, 0, z));
            } else {
                int x = GetMyGoalPosition()[0] > 0 ? -4 : 4;
                int z = GetBallPosition()[2] > 0 ? -6 : 6;
                GoTo(new Vector3(x, 0, z));
            }
        }

        public override void OnReachBall()
        {
            if (CanShoot(GetRivalGoalPosition(), 0.4f)) {
                ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
                Debug.Log("Mate: Tiro al Arco");
            } else if (CanShoot(GetTeamMatesInformation()[1].Position, 0.3f)) {
                Vector3 pos = GetTeamMatesInformation()[1].Position;
                ShootBall(GetDirectionTo(pos), GetForce(pos));
                Debug.Log("Mate: Pase a German");
            } else if (CanShoot(GetTeamMatesInformation()[0].Position, 1.0f)) {
                Vector3 pos = GetTeamMatesInformation()[0].Position;
                ShootBall(GetDirectionTo(pos), GetForce(pos));
                Debug.Log("Mate: Pase a Kippur");
            } else {
                ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
                Debug.Log("Mate: Tiro al Arco");
            }
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public override FieldPosition GetInitialPosition() => FieldPosition.C3;

        public override string GetPlayerDisplayName() => "Mate";
    }
}