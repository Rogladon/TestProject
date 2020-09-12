using Core;

namespace Conf {
	public class MeleeMonkUnitInfo : UnitInfo {
		public IBuff ShieldBuff;
		public IBuff HellengBuff;

		public MeleeMonkUnitInfo() {
			MaxHealth = 1800;
			MaxMana = 100;
			Speed = 3;
			Damage = 5;
			ManaRegen = 5;
			AttackDistance = 2;
			ShieldBuff = new Logic.ShieldBuff(int.MaxValue, 100, 250);
			HellengBuff = new Logic.HeelengPrecentDamageBuff(int.MaxValue, 10);
		}
	}
}
