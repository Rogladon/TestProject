using Core;

namespace Logic {
	public class ShieldBuff : IBuff {
		private int _shieldStrength;
		private readonly int _damage;

		public ShieldBuff(int lifeTick, int shieldStrength, int shieldExplosionDamage) : base(lifeTick) {
			this._shieldStrength = shieldStrength;
			_damage = shieldExplosionDamage;
		}

		private ShieldBuff(ShieldBuff buff, IUnit unit, ICore core) : base(buff, unit, core) {
			_shieldStrength = buff._shieldStrength;
		}

		public override int OnDamage(int damage) {
			damage = (int)System.Math.Round((double)(damage/2));
			_shieldStrength -= damage;
			if(_shieldStrength <= 0) {
				Unit.RemoveBuff(this);
			}
			return damage;
		}

		public override int OnBeforeManaChange(int delta) {
			_shieldStrength += delta;
			return delta;
		}

		public override void OnDie() {
			var target = Core.GetNearestFriend(Unit);
			target.Heal(_shieldStrength);
		}

		public override void EndBuff() {
			var target = Core.GetNearestEnemy(Unit);
			target.Damage(_damage);
			Unit.Heal(100);
		}

		public override IBuff Copy(IUnit unit, ICore core) {
			return new ShieldBuff(this, unit, core);
		}
	}
}
