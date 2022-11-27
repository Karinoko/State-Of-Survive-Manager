using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace State_Of_Survive_Manager
{
    public class Units
    {
        Types.TierUnit Tier;
        Types.UnitType unitType;
        public int Power { get { return _Power; } }
        int _Power;
        public int HP { get { return _HP; } }
        int _HP;
        public int Attack { get { return _Attack; } }
        int _Attack;
        public int Defence { get { return _Defence; } }
        int _Defence;

        public Units(int AmountOfUnit, Types.TierUnit tierUnit = Types.TierUnit.T1, Types.UnitType unitType = Types.UnitType.INFANTRY)
        {
            switch (unitType)
            {
                case Types.UnitType.INFANTRY:
                    {
                        _Power = 3;
                        _HP = 6;
                        _Attack = 1;
                        _Defence = 4;
                    }
                    break;
                case Types.UnitType.HUNTERS:
                    {
                        _Power = 3;
                        _HP = 1;
                        _Attack = 5;
                        _Defence = 1;
                    }
                    break;
                case Types.UnitType.RIDERS:
                    {
                        _Power = 3;
                        _HP = 2;
                        _Attack = 4;
                        _Defence = 2;
                    }
                    break;
            }

            _HP += (int)tierUnit - 1;
            _Attack += (int)tierUnit - 1;
            _Defence += (int)tierUnit - 1;

            switch (tierUnit)
            {
                case Types.TierUnit.T1:
                    _Power += 0;
                    break;
                case Types.TierUnit.T2:
                    _Power += 1;
                    break;
                case Types.TierUnit.T3:
                    _Power += 3;
                    break;
                case Types.TierUnit.T4:
                    _Power += 6;
                    break;
                case Types.TierUnit.T5:
                    _Power += 10;
                    break;
                case Types.TierUnit.T6:
                    _Power += 17;
                    break;
                case Types.TierUnit.T7:
                    _Power += 25;
                    break;
                case Types.TierUnit.T8:
                    _Power += 35;
                    break;
                case Types.TierUnit.T9:
                    _Power += 47;
                    break;
                case Types.TierUnit.T10:
                    _Power += 63;
                    break;
            }

            _Power *= AmountOfUnit;
            _HP *= AmountOfUnit;
            _Attack *= AmountOfUnit;
            _Defence *= AmountOfUnit;
        }
    }
}
