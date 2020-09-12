using System.Collections;
using System.Collections.Generic;
using Core;

namespace Logic {
	public class StunningDeBuff : IBuff {
		private int _healthUnit;

		public StunningDeBuff(int lifeTick) : base(lifeTick) {
		}

		private StunningDeBuff(StunningDeBuff buff, IUnit unit, ICore core) : base(buff, unit, core) {
		}

		public override void StartBuff() {
			Unit.LockAction = true;
			_healthUnit = Unit.Health;
		}
		public override void EndBuff() {
			Unit.LockAction = false;
		}

		public override void PostLogicTick() {
			if (Unit.Health < _healthUnit - 100) {
				Unit.RemoveBuff(this);
				return;
			}
			_healthUnit = Unit.Health;
		}

		public override IBuff Copy(IUnit unit, ICore core) {
			return new StunningDeBuff(this, unit, core);
		}
	}
}
