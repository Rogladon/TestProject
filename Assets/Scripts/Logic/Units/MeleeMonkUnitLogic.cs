using Conf;
using Core;

namespace Logic {
	public class MeleeMonkUnitLogic : UnitLogic {
		private readonly int _attackDistance;
		private readonly int _damage;
		private readonly int _manaRegen;
		private readonly IBuff _shieldBuff;
		private readonly IBuff _heelengBuff;

		public MeleeMonkUnitLogic(MeleeMonkUnitInfo info, IUnit unit, ICore core) : base(unit, core) {
			_damage = info.Damage;
			_manaRegen = info.ManaRegen;
			_attackDistance = info.AttackDistance;
			_shieldBuff = info.ShieldBuff;
			_heelengBuff = info.HellengBuff;

		}

		public override void OnSpawn() {
			Unit.AddBuff(_shieldBuff);
			Unit.AddBuff(_heelengBuff);
		}

		public override void OnTurn() {
			var target = Core.GetNearestEnemy(Unit);
			if (target != null && target.IsAlive()) {
				if (Core.GetDistance(Unit, target) > _attackDistance) {
					Unit.MoveTo(target.X, target.Y);
				} else {
					target.Damage(_damage);
				}
			}
			Unit.AddMana(_manaRegen);
		}
	}
}