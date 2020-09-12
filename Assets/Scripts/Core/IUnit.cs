namespace Core
{
    public interface IUnit
    {
        TeamFlag Team { get; }
        
        int X { get; }
        int Y { get; }
        
        int MaxHealth { get; }
        int Health { get; }
        int MaxMana { get; }
        int Mana { get; }
		bool LockAction { get; set; }
		System.Collections.Generic.List<IBuff> Buffs { get; }
        bool IsAlive();
        void AddMana(int mana);
        void SubMana(int mana);
        void Heal(int heal);
        void Damage(int damage);
        void MoveTo(int x, int y);
		void AddBuff(IBuff buff);
		void RemoveBuff(IBuff buff);
    }
}