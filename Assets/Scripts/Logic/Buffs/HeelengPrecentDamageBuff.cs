using Core;

namespace Logic {
	public class HeelengPrecentDamageBuff : IBuff {
		private int lasthealthUnit;
		private int healPrecent;
		public HeelengPrecentDamageBuff(int lifeTick, int _healPrecent) : base(lifeTick) {
			healPrecent = _healPrecent;
		}

		private HeelengPrecentDamageBuff(HeelengPrecentDamageBuff buff, IUnit unit, ICore core) : base(buff, unit, core) {
			numberOfLifeTick = buff.numberOfLifeTick;
		}

		public override void StartBuff() {
			lasthealthUnit = Unit.Health;
		}

		public override void PostLogicTick() {
			if(lasthealthUnit > Unit.Health) {
				int healengValue = (int)System.Math.Round((double)((lasthealthUnit - Unit.Health) * healPrecent / 100));
				if (Unit.LockAction) {
					Unit.LockAction = false;
					Unit.Heal(healPrecent);
					Unit.LockAction = true;
				} else {
					Unit.Heal(healPrecent);
				}
			}
		}

		public override IBuff Copy(IUnit unit, ICore core) {
			return new HeelengPrecentDamageBuff(this, unit, core);
		}
	}
}
