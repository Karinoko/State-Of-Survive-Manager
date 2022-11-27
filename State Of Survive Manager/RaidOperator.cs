using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace State_Of_Survive_Manager
{
    /// <summary>
    /// Głowna klasa operatora rajdów
    /// Przeprowadza działania powiązane z symulacją walki z bossem
    /// </summary>
    public class RaidOperator
    {
        /// <summary>
        /// Punkty życia bossa
        /// </summary>
        int BossHp;

        /// <summary>
        /// Pojemność rajdu
        /// </summary>
        int RaidCapacity;

        /// <summary>
        /// Obrażenia zadane przez raid
        /// </summary>
        int RaidDamage;

        /// <summary>
        /// Ilość potencjalnych uczestników
        /// </summary>
        int AmountPlayers;

        /// <summary>
        /// Dopuszczalna ilość jednostek na gracza
        /// </summary>
        int AmountUnits;

        /// <summary>
        /// Czy rajdy będą równoległe - aktywuje fale szturmów
        /// </summary>
        bool IsParallel = true;

        /// <summary>
        /// Inicjuje wystąpienie klasy
        /// </summary>
        /// <param name="bossHP">Ilość HP Bossa</param>
        /// <param name="raidCapacity">Pojemność rajdu</param>
        /// <param name="raidDamage">Obrażenia rajdu</param>
        /// <param name="amountPlayers">Ilość graczy</param>
        /// <param name="amountsUnits">Ilość jednostek</param>
        /// <param name="isParallel">Dozwolone rajdy równoległe</param>
        public RaidOperator(int bossHP, int raidCapacity, int raidDamage, int amountPlayers, int amountsUnits, bool isParallel = true, bool toUsers = false, bool isAuto = false)
        {
            BossHp = bossHP;
            RaidCapacity = raidCapacity;
            RaidDamage = raidDamage;
            AmountPlayers = amountPlayers;
            AmountUnits = amountsUnits;

            IsParallel = isParallel;

            PrepareAmountsOfRaids(toUsers, isAuto);
        }

        /*
         *  Działania na wprowadzonych danych
         *  Symulacja obejmuje dane:
         *  
         *  HP Bossa = 500 000
         *  Pojemność rajdu = 300 000
         *  Obrażenia rajdu = 190 000
         *  Ilość graczy = 12
         *  Jednostek = 50 000
         *  
         *  Tryb równoległy
         */

        /// <summary>
        /// Ilość raidów
        /// </summary>
        int _AmountRaids = 0;
        /// <summary>
        /// Ilość rajdów awaryjnych
        /// </summary>
        int _AmountEmrgencyRaids = 0;
        /// <summary>
        /// Graczy na rajd
        /// </summary>
        int _PlayerPerRaid = 0;
        /// <summary>
        /// Maksymum graczy
        /// </summary>
        int _MaxPlayer = 0;
        /// <summary>
        /// Wolne miejsca
        /// </summary>
        int _FreeSlots = 0;
        /// <summary>
        /// Ilość fal
        /// </summary>
        int _PossibleRaids = 1;
        /// <summary>
        /// Ilość jednostek w rajdzie awaryjnym
        /// </summary>
        int _EmergencyAmountUnit = 0;
        /// <summary>
        /// Maksymalna ilość raidów
        /// </summary>
        int _MaxRaids = 0;
        /// <summary>
        /// Ilość jednostek na gracza w rajdzie
        /// </summary>
        int _AmountUnitsPerPlayer = 0;
        /// <summary>
        /// Iliść potencjalnych uczestników
        /// </summary>
        double _AmountPlayers = 0;

        /// <summary>
        /// Przeprowadza symulacje na podstawie dostarczonych danych
        /// Tryb automatyczny przeprowadza symulację z HP Bossa oraz średniej pojemności rajdu
        /// Tryb manualny przeprowadza symulację ze wszystkich kontrolek
        /// </summary>
        /// <param name="toUsers">Nacisk na ilość osób, false dla 60% domyślnej ilości osób mogących brać udział w rajdzie</param>
        /// <param name="isAuto">True jeśli symulacja ma zostać przeprowadzona dla HP bossa na podstawie pojemności</param>
        void PrepareAmountsOfRaids(bool toUsers = false, bool isAuto = false)                                   /// Auto raid                           Manual raid
        {
            var variantA = (int)Math.Ceiling((double)BossHp / RaidCapacity);                                    /// 2           [500 000 / 300 000]
            var variantB = Math.Ceiling((double)RaidCapacity / variantA);                                       /// 150 000     [300 / 2]
            var variantC = (int)Math.Ceiling(BossHp / variantB);                                                /// 4           [500 000 / 150 000]
            if (toUsers)
                _AmountPlayers = (isAuto) ? variantA * variantC : AmountPlayers;                                /// 8           [2 * 4]                 12
            else
                _AmountPlayers = (isAuto) ? (int)Math.Ceiling(variantA * variantC * 0.6) : AmountPlayers;

            _MaxRaids = (isAuto) ? variantA : BossHp / RaidDamage;                                              /// 2 == 500 000 / 300 000              10 == 500 000 / 50 000
            _AmountUnitsPerPlayer = (isAuto) ? (int)Math.Ceiling(RaidCapacity / _AmountPlayers) : AmountUnits;  /// 37 500 == 300 000 / 8               50 000

            _PlayerPerRaid = RaidCapacity / _AmountUnitsPerPlayer;                                              /// 8 == 300 000 / 37 500               6 == 300 000 / 50 000
            
            // Przypadek kiedy aktywne jest: Szturmy Równoległe
            if (IsParallel)
                PrepareParallelRaid();
            else
                PrepareNonParallelRaid();
                
            _MaxPlayer = _AmountRaids * _PlayerPerRaid;                                                         /// 8 == 1 * 8                          12 == 2 * 6
            _FreeSlots = _MaxPlayer - (int)_AmountPlayers;                                                      /// 0 == 8 - 8                          0 == 12 - 12
        }

        /// <summary>
        /// Przygotowywuje dane pod raid równoległy
        /// </summary>
        void PrepareParallelRaid()
        {
            _AmountRaids = int.Parse(Math.Ceiling(_AmountPlayers / _PlayerPerRaid).ToString());                 /// 1 == 8 / 8                          2 == 12 / 6
            _PossibleRaids = _MaxRaids / _AmountRaids;                                                          /// 2 == 2 / 1                          5 == 10 / 2
            _AmountEmrgencyRaids = _MaxRaids % _AmountRaids;                                                    /// 0 == 2 % 1                          0 == 10 % 2
            _EmergencyAmountUnit = (_AmountEmrgencyRaids > 0) ? RaidCapacity / AmountPlayers : 0;               /// 0                                   25 000 == 300 000 / 12
        }

        /// <summary>
        /// Przygotowywuje dane pod zwykłe raidy
        /// </summary>
        void PrepareNonParallelRaid()
        {
            _AmountRaids = _MaxRaids;
            _PossibleRaids = _MaxRaids / _AmountRaids;
        }

        /// <summary>
        /// Zwraca ilość wymaganych rajdów
        /// </summary>
        /// <returns>Ilość rajdów</returns>
        public string GetAmountRaids()
        {
            return (IsParallel) ? ($"{_AmountRaids} / {_MaxRaids}") : _AmountRaids.ToString();
        }

        /// <summary>
        /// Zwraca dopuszczalną ilość osób w szturnie
        /// </summary>
        /// <returns>Ilość osób</returns>
        public int GetAmountPlayers()
        {
            return _PlayerPerRaid;
        }

        /// <summary>
        /// Zwraca ilość jednostek na gracza
        /// </summary>
        /// <returns>Ilość jednostek</returns>
        public int GetAmountUnits()
        {
            return _AmountUnitsPerPlayer;
        }

        /// <summary>
        /// Zwraca ilość jednostek w szturmie awaryjnym
        /// </summary>
        /// <returns>Ilość jednostek</returns>
        public int GetAmountEmergency()
        {
            return _EmergencyAmountUnit;
        }

        /// <summary>
        /// Zrwaca maksymalna ilość obsługiwach członków szturmów
        /// </summary>
        /// <returns>Maksymalna pojemność przez wszystkie rajdy</returns>
        public int GetMaxPlayes()
        {
            return _MaxPlayer;
        }

        /// <summary>
        /// Zwraca ilość wolnych miejsc w szturmach
        /// </summary>
        /// <returns>Ilość wolnych slotów</returns>
        public int GetFreeSlots()
        {
            return _FreeSlots;
        }

        /// <summary>
        /// Zwraca ilość fal szturmów do wykonania
        /// </summary>
        /// <returns></returns>
        public int GetPossibleRaids()
        {
            return _PossibleRaids;
        }

        /// <summary>
        /// Ilość awaryjnych szturmów
        /// </summary>
        /// <returns>Olość awaryjnych rajdów</returns>
        public int GetPossibleEmergency()
        {
            return _AmountEmrgencyRaids;
        }
    }
}
