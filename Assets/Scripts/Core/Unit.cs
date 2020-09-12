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
            Die,
			Buff
        }

        private readonly UnitInfo _info;
        private readonly UnitLogic _logic;
        private readonly Battle _battle;
        
        private State _state;
        
        private int _destX;
        private int _destY;
		private List<IBuff> buffs = new List<IBuff>();
        
        public readonly UnitView View;
        
        public TeamFlag Team { get; }
        
        public int X { get; set; }
        public int Y { get; set; }

        public int MaxHealth => _info.MaxHealth;
        public int Health { get; private set; }
        public int MaxMana => _info.MaxMana;
        public int Mana { get; private set; }
        public int Speed => _info.Speed;
		public List<IBuff> Buffs => buffs;


		public Unit(TeamFlag team, UnitInfo info, UnitView view, Battle battle)
        {
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
            switch (_state)
            {
                case State.Spawn:
                    _logic.OnSpawn();
                    _state = State.Turn;
                    break;
                case State.Move:
                    _battle.AskMoveUnitTo(this, _destX, _destY);
                    _state = State.Turn;
                    break;
                case State.Turn:
                    if (Mana == MaxMana)
                    {
                        _logic.OnAbility();
                        SubMana(MaxMana);
                        break;
                    }
                    _logic.OnTurn();
                    break;
                case State.Die:
                    _logic.OnDie();
                    break;
				case State.Buff:
					if(buffs.Count == 0) {
						_state = State.Turn;
						break;
					}
					for(int i = 0; i < buffs.Count; i++) {
						buffs[i].Tick(this);
					}
					break;
            }
        }

        public bool IsAlive()
        {
            return Health > 0;
        }
        
        public void AddMana(int mana)
        {
            mana = _logic.OnBeforeManaChange(mana);
            Mana = Math.Min(MaxMana, Mana + mana);
        }
        
        public void SubMana(int mana)
        {
            mana = -_logic.OnBeforeManaChange(-mana);
            Mana = Math.Min(MaxMana, Mana - mana);
        }
        
        public void Heal(int heal)
        {
            heal = _logic.OnHeal(heal);
            Health = Math.Min(MaxHealth, Health + heal);
        }

        public void Damage(int damage)
        {
            damage = _logic.OnDamage(damage);
            Health = Math.Max(0, Health - damage);
            if (!IsAlive())
            {
                _state = State.Die;
            }
        }

        public void MoveTo(int x, int y)
        {
            _destX = x;
            _destY = y;
            _state = State.Move;
        }

		public void AddBuff(IBuff buff) {
			IBuff repeatBuff = null;
			foreach(var b in buffs) {
				if(b.GetType() == buff.GetType()) {
					repeatBuff = b;
					break;
				}
			}
			if(repeatBuff != null) {
				buffs.Remove(repeatBuff);
			}
			IBuff newBuff = buff.Copy();
			buffs.Add(newBuff);
			newBuff.StartBuff(this);
			_state = State.Buff;
		}

		public void RemoveBuff(IBuff buff) {
			buff.EndBuff(this);
			buffs.Remove(buff);
			if (buffs.Count == 0) {
				_state = State.Turn;
			}
		}
	}
}