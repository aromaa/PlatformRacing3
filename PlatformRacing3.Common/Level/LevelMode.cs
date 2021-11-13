using NpgsqlTypes;

namespace PlatformRacing3.Common.Level;

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
	[PgName("damageDash")]
	DamageDash,
	[PgName("kingOfTheHat")]
	KingOfTheHat,
}