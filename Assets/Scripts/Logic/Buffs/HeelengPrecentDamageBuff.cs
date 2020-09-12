using Core;

namespace Logic {
	public class HeelengPrecentDamageBuff : IBuff {
		private int _lasthealthUnit;
		private readonly int _healPrecent;
		public HeelengPrecentDamageBuff(int lifeTick, int healPrecent) : base(lifeTick) {
			_healPrecent = healPrecent;
		}

		private HeelengPrecentDamageBuff(HeelengPrecentDamageBuff buff, IUnit unit, ICore core) : base(buff, unit, core) {
			_healPrecent = buff._healPrecent;
		}

		public override void StartBuff() {
			_lasthealthUnit = Unit.Health;
		}

		public override void PostLogicTick() {
			if(_lasthealthUnit > Unit.Health) {
				int healengValue = (int)System.Math.Round((double)((_lasthealthUnit - Unit.Health) * _healPrecent / 100));
				if (Unit.LockAction) {
					Unit.LockAction = false;
					Unit.Heal(_healPrecent);
					Unit.LockAction = true;
				} else {
					Unit.Heal(_healPrecent);
				}
			}
		}

		public override IBuff Copy(IUnit unit, ICore core) {
			return new HeelengPrecentDamageBuff(this, unit, core);
		}
	}
}
