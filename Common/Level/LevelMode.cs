using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Common.Level
{
    public enum LevelMode
    {
        [PgName("race")]
        Race,
        [PgName("deathmatch")]
        Deathmatch,
        [PgName("hatAttack")]
        HatAttack,
        [PgName("coinFiend")]
        CoinFiend,
        [PgName("kingOfTheHat")]
        KingOfTheHat,
    }
}
