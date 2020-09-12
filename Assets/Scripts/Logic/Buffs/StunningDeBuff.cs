using System.Collections;
using System.Collections.Generic;
using Core;

namespace Logic {
	public class StunningDeBuff : IBuff {
		private int healthUnit;

		public StunningDeBuff(int lifeTick) : base(lifeTick) {
			numberOfLifeTick = lifeTick;
		}

		private StunningDeBuff(StunningDeBuff buff) : base(buff) {
			numberOfLifeTick = buff.numberOfLifeTick;
		}

		public override void StartBuff(IUnit unit) {
			healthUnit = unit.Health;
		}

		public override void LogicTick(IUnit unit) {
			if (unit.Health < healthUnit - 100) {
				unit.RemoveBuff(this);
				return;
			}
			healthUnit = unit.Health;
		}

		public override IBuff Copy() {
			return new StunningDeBuff(this);
		}

		
	}
}
