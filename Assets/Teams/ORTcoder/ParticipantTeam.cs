using Core.Games;
using Core.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Teams.ORTcoder
{
    [UsedImplicitly]
    public class ExampleTeam : Team
    {
        public TargetPreference PenaltyDivePreference { get; } = new TargetPreference(3,1,4);

        public TargetPreference PenaltyKickPreference { get; } = new TargetPreference(3,4,2);
        
        public TeamPlayer GetPlayerOne() => new PlayerOne();

        public TeamPlayer GetPlayerTwo() => new PlayerTwo();

        public TeamPlayer GetPlayerThree() => new PlayerThree();
        
        public Color PrimaryColor => new Color(0.0f, 0.0f, 0.6f);

        public string GetName() => "ORTcoder";

        public string TeamShield => "Blue";
    }
}