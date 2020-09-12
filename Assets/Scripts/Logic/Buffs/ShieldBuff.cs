using Core;

namespace Logic {
	public class ShieldBuff : IBuff {
		private int shieldStrength;
		private int damage;

		public ShieldBuff(int lifeTick, int _shieldStrength, int _shieldExplosionDamage) : base(lifeTick) {
			shieldStrength = _shieldStrength;
			damage = _shieldExplosionDamage;
		}

		private ShieldBuff(ShieldBuff buff, IUnit unit, ICore core) : base(buff, unit, core) {
			shieldStrength = buff.shieldStrength;
		}

		public override int OnDamage(int damage) {
			damage = (int)System.Math.Round((double)(damage/2));
			shieldStrength -= damage;
			if(shieldStrength <= 0) {
				Unit.RemoveBuff(this);
			}
			return damage;
		}

		public override int OnBeforeManaChange(int delta) {
			shieldStrength += delta;
			return delta;
		}

		public override void OnDie() {
			var target = Core.GetNearestFriend(Unit);
			target.Heal(shieldStrength);
		}

		public override void EndBuff() {
			var target = Core.GetNearestEnemy(Unit);
			target.Damage(damage);
			Unit.Heal(100);
		}

		public override IBuff Copy(IUnit unit, ICore core) {
			return new ShieldBuff(this, unit, core);
		}
	}
}
