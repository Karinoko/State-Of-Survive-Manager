using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace State_Of_Survive_Manager
{
    public class Record
    {
        /// <summary>
        /// Punkty życia bossa
        /// </summary>
        public string BossHp;

        /// <summary>
        /// Pojemność rajdu
        /// </summary>
        public string RaidCapacity;

        /// <summary>
        /// Obrażenia zadane przez raid
        /// </summary>
        public string RaidDamage;

        /// <summary>
        /// Zakres obrażeń minimalnych
        /// </summary>
        public string RaidDamageMin;

        /// <summary>
        /// Zakres obrażeń maksymalnych
        /// </summary>
        public string RaidDamageMax;

        /// <summary>
        /// Ilość potencjalnych uczestników
        /// </summary>
        public string AmountPlayers;

        /// <summary>
        /// Dopuszczalna ilość jednostek na gracza
        /// </summary>
        public string AmountUnits;

        /// <summary>
        /// Zakres minimalny pojemności marszu
        /// </summary>
        public string AmountUnitsMin;

        /// <summary>
        /// Zakres maksymalny pojemności marszu
        /// </summary>
        public string AmountUnitsMax;

        /// <summary>
        /// Czy rajdy będą równoległe - aktywuje fale szturmów
        /// </summary>
        public bool IsParallel = true;
    }

    public class ConfigReader
    {
        public static void SaveConfiguration(Record record)
        {
            Stream writer;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileDialog.Filter = "Raid Config Files (*.rcf)|*.rcf|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            var toWrite = ($"{record.BossHp};{record.RaidCapacity};{record.RaidDamage};{record.RaidDamageMin};{record.RaidDamageMax};{record.AmountPlayers};{record.AmountUnits};{record.AmountUnitsMin};{record.AmountUnitsMax};{record.IsParallel}");

            int count = toWrite.Length;
            char[] tmp = new char[count];
            tmp = toWrite.ToCharArray();

            byte[] buffor = new byte[count];

            for (int i = 0; i < count; i++)
            {
                buffor[i] = Convert.ToByte(tmp[i]);
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((writer = saveFileDialog.OpenFile()) != null)
                {
                    writer.Write(buffor, 0, count);
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Odczytuje zawartość pliku używając specyficznego okna dialogowego
        /// </summary>
        /// <returns>Record z danymi do wypełnienia kontrolek</returns>
        public static Record ReadConfiguration()
        {
            string fileContent = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog.Filter = "Raid Config Files (*.rcf)|*.rcf|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            var values = fileContent.Split(';');
            Record record = new Record()
            {
                BossHp = values[0],
                RaidCapacity = values[1],
                RaidDamage = values[2],
                RaidDamageMin = values[3],
                RaidDamageMax = values[4],
                AmountPlayers = values[5],
                AmountUnits = values[6],
                AmountUnitsMin = values[7],
                AmountUnitsMax = values[8],
                IsParallel = Convert.ToBoolean(values[9])
            };

            return record;
        }
    }
}
