using System;
using System.Collections.Generic;
using Conf;
using Logic;
using View;

namespace Core
{
    public class Unit : IUnit
    {
        private enum State
        {
            Spawn,
            Turn,
            Move,
            Die
        }

        private readonly UnitInfo _info;
        private readonly UnitLogic _logic;
        private readonly Battle _battle;
        
        private State _state;
        
        private int _destX;
        private int _destY;
        
        public readonly UnitView View;
        
        public TeamFlag Team { get; }
        
        public int X { get; set; }
        public int Y { get; set; }

        public int MaxHealth => _info.MaxHealth;
        public int Health { get; private set; }
        public int MaxMana => _info.MaxMana;
        public int Mana { get; private set; }
        public int Speed => _info.Speed;
		public bool LockAction { get; set; }

		public List<IBuff> Buffs { get; private set; } = new List<IBuff>();

		public Unit(TeamFlag team, UnitInfo info, UnitView view, Battle battle)
        {
			LockAction = false;
            Team = team;
            _info = info;
            
            Health = MaxHealth;
            Mana = 0;
            
            _logic = UnitLogicFactory.Create(info, this, battle);
            
            View = view;
            _battle = battle;
            _state = State.Spawn;
        }

        public void Tick()
        {
			if (Buffs.Count != 0) {
				for (int i = 0; i < Buffs.Count; i++) {
					Buffs[i].PreTick();
				}
			}
			switch (_state)
            {
                case State.Spawn:
                    _logic.OnSpawn();
                    _state = State.Turn;
                    break;
                case State.Move:
					if (LockAction) break;
                    _battle.AskMoveUnitTo(this, _destX, _destY);
                    _state = State.Turn;
                    break;
                case State.Turn:
					if (LockAction) break;
                    if (Mana == MaxMana)
                    {
                        _logic.OnAbility();
                        SubMana(MaxMana);
                        break;
                    }
                    _logic.OnTurn();
                    break;
                case State.Die:
                    OnDie();
                    break;
            }
			if (!IsAlive()) return;
			if (Buffs.Count != 0) {
				for (int i = 0; i < Buffs.Count; i++) {
					Buffs[i].PostTick();
				}
			}
		}

        public bool IsAlive()
        {
            return Health > 0;
        }
        
        public void AddMana(int mana)
        {
			for (int i = 0; i < Buffs.Count; i++) {
				mana = Buffs[i].OnBeforeManaChange(mana);
			}
			if (LockAction) return;
            mana = _logic.OnBeforeManaChange(mana);
            Mana = Math.Min(MaxMana, Mana + mana);
        }
        
        public void SubMana(int mana)
        {
			for (int i = 0; i < Buffs.Count; i++) {
				mana = Buffs[i].OnBeforeManaChange(-mana);
			}
			if (LockAction) return;
			mana = -_logic.OnBeforeManaChange(-mana);
            Mana = Math.Min(MaxMana, Mana - mana);
        }
        
        public void Heal(int heal)
        {
			if (LockAction) return;
			heal = _logic.OnHeal(heal);
            Health = Math.Min(MaxHealth, Health + heal);
        }

        public void Damage(int damage)
        {
			for (int i = 0; i < Buffs.Count; i++) {
				damage = Buffs[i].OnDamage(damage);
			}
			damage = _logic.OnDamage(damage);
            Health = Math.Max(0, Health - damage);
            if (!IsAlive())
            {
                _state = State.Die;
            }
        }

        public void MoveTo(int x, int y)
        {
			if (LockAction) return;
			_destX = x;
            _destY = y;
            _state = State.Move;
        }

		public void OnDie() {
			_logic.OnDie();
			for (int i = 0; i < Buffs.Count; i++) {
				Buffs[i].OnDie();
			}
		}

		public void AddBuff(IBuff buff) {
			IBuff repeatBuff = null;
			for (int i = 0; i < Buffs.Count; i++) {
				if(Buffs[i].GetType() == buff.GetType()) {
					repeatBuff = Buffs[i];
					break;
				}
			}
			if(repeatBuff != null) {
				Buffs.Remove(repeatBuff);
			}
			IBuff newBuff = buff.Copy(this, _battle);
			Buffs.Add(newBuff);
			newBuff.StartBuff();
		}

		public void RemoveBuff(IBuff buff) {
			buff.EndBuff();
			Buffs.Remove(buff);
		}
	}
}