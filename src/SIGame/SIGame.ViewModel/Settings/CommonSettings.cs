﻿using SICore;
using SICore.Results;
using SIData;
using SIGame.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace SIGame
{
    /// <summary>
    /// Общие настройки для всех пользователей
    /// </summary>
    public sealed class CommonSettings
    {
        /// <summary>
        /// Имя игры
        /// </summary>
        public const string AppName = "SIGame";

        /// <summary>
        /// Имя производителя (англ.)
        /// </summary>
        public const string ManufacturerEn = "Khil-soft";
        /// <summary>
        /// Имя игры (англ.)
        /// </summary>
        public const string AppNameEn = "SIGame";

        public const string LogsFolderName = "Logs";

        internal const string OnlineGameUrl = "https://vladimirkhil.com/si/online?gameId=";

        /// <summary>
        /// Экземпляр общих настроек
        /// </summary>
        public static CommonSettings Default { get; set; }

        #region Settings
        /// <summary>
        /// Зарегистрированные учётные записи пользователей
        /// </summary>
        public List<HumanAccount> Humans2 { get; set; }
        /// <summary>
        /// Зарегистрированные учётные записи пользователей
        /// </summary>
        public ObservableCollection<HumanAccount> Humans { get; set; }
        /// <summary>
        /// Зарегистрированные компьютерные игроки
        /// </summary>
        public ObservableCollection<ComputerAccount> CompPlayers { get; set; }
        /// <summary>
        /// Зарегистрированные компьютерные игроки
        /// </summary>
        public List<ComputerAccount> CompPlayers2 { get; set; }
        /// <summary>
        /// Лучшие игроки
        /// </summary>
        public ObservableCollection<BestPlayer> BestPlayers { get; set; }

        /// <summary>
        /// Компьютерные ведущие
        /// </summary>
        public ObservableCollection<ComputerAccount> CompShowmans { get; set; }
        /// <summary>
        /// Компьютерные ведущие
        /// </summary>
        public List<ComputerAccount> CompShowmans2 { get; set; }

        /// <summary>
        /// Delayed game results.
        /// </summary>
        public List<SI.GameResultService.Client.GameResult> DelayedResultsNew { get; set; }

        /// <summary>
        /// Отложенные отчёты об ошибках
        /// </summary>
        public ErrorInfoList DelayedErrorsNew { get; set; }

        #endregion

        public CommonSettings()
        {
            Humans = new ObservableCollection<HumanAccount>();
            Humans2 = new List<HumanAccount>();
            CompPlayers = new ObservableCollection<ComputerAccount>();
            CompPlayers2 = new List<ComputerAccount>();
            CompShowmans = new ObservableCollection<ComputerAccount>();
            CompShowmans2 = new List<ComputerAccount>();
            BestPlayers = new ObservableCollection<BestPlayer>();
            DelayedErrorsNew = new ErrorInfoList();
            DelayedResultsNew = new List<SI.GameResultService.Client.GameResult>();
        }

        public void Save(Stream stream, XmlSerializer serializer = null)
        {
            if (serializer == null)
                serializer = new XmlSerializer(typeof(CommonSettings));

            serializer.Serialize(stream, this);
        }

        /// <summary>
        /// Загрузить общие настройки игры
        /// </summary>
        /// <returns>Загруженные общие настройки или настройки по умолчанию (если загрузить сохранённые настройки не удалось)</returns>
        public static CommonSettings LoadOld(string configFileName)
        {
            using (var file = IsolatedStorageFile.GetMachineStoreForAssembly())
            {
                if (file.FileExists(configFileName) && Monitor.TryEnter(configFileName, 2000))
                {
                    try
                    {
                        using (var stream = file.OpenFile(configFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            return Load(stream);
                        }
                    }
                    catch { }
                    finally
                    {
                        Monitor.Exit(configFileName);
                    }
                }
            }

            return null;
        }

        public static CommonSettings Load(Stream stream, XmlSerializer serializer = null)
        {
            if (serializer == null)
                serializer = new XmlSerializer(typeof(CommonSettings));

            var settings = (CommonSettings)serializer.Deserialize(stream);

            if (settings.CompPlayers.Any())
            {
                settings.CompPlayers2.AddRange(settings.CompPlayers.Where(compPlayer => compPlayer.CanBeDeleted));
                settings.CompPlayers.Clear();
            }

            if (settings.CompShowmans.Any())
            {
                settings.CompShowmans2.AddRange(settings.CompShowmans.Where(compShowman => compShowman.CanBeDeleted));
                settings.CompShowmans.Clear();
            }

            if (settings.Humans.Any() && !settings.Humans2.Any())
            {
                settings.Humans2.AddRange(settings.Humans.Where(human => human.CanBeDeleted));
            }

            return settings;
        }

        internal void LoadFrom(Stream stream)
        {
            var settings = Load(stream);

            foreach (var item in settings.Humans2)
            {
                var human = Humans2.FirstOrDefault(h => h.Name == item.Name);
                if (human == null)
                {
                    Humans2.Add(item);
                }
                else if (human.CanBeDeleted)
                {
                    human.IsMale = item.IsMale;
                    human.Picture = item.Picture;
                }
            }

            foreach (var item in settings.CompPlayers2)
            {
                var comp = CompPlayers2.FirstOrDefault(c => c.Name == item.Name);
                if (comp == null)
                {
                    CompPlayers2.Add(item);
                }
                else if (comp.CanBeDeleted)
                {
                    comp.LoadInfo(item);
                }
            }
        }
    }
}
