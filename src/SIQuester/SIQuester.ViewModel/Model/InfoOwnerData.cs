﻿using SIPackages;
using SIPackages.Core;
using SIQuester.ViewModel;
using System.Text;
using System.Xml;

namespace SIQuester.Model;

/// <summary>
/// Contains package object data used in copy-paste and drag-and-drop operations.
/// </summary>
[Serializable]
public sealed class InfoOwnerData
{
    public enum Level { Package, Round, Theme, Question };

    public Level ItemLevel { get; set; }

    public string ItemData { get; set; }

    public AuthorInfo[] Authors { get; set; }

    public SourceInfo[] Sources { get; set; }

    public Dictionary<string, string> Images { get; set; } = new();

    public Dictionary<string, string> Audio { get; set; } = new();

    public Dictionary<string, string> Video { get; set; } = new();

    public InfoOwnerData(QDocument document, IItemViewModel item)
    {
        var model = item.GetModel();

        var sb = new StringBuilder();

        using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true }))
        {
            model.WriteXml(writer);
        }

        ItemData = sb.ToString();

        ItemLevel =
            model is Package ? Level.Package :
            model is Round ? Level.Round :
            model is Theme ? Level.Theme : Level.Question;

        GetFullData(document, item);
    }

    public InfoOwner GetItem()
    {
        InfoOwner item = ItemLevel switch
        {
            Level.Package => new Package(),
            Level.Round => new Round(),
            Level.Theme => new Theme(),
            _ => new Question(),
        };

        using (var sr = new StringReader(ItemData))
        {
            using var reader = XmlReader.Create(sr);
            reader.Read();
            item.ReadXml(reader);
        }

        return item;
    }

    /// <summary>
    /// Gets full object data including attached objects.
    /// </summary>
    /// <param name="document">Document which contains the object.</param>
    /// <param name="owner">Object having necessary data.</param>
    private void GetFullData(QDocument documentViewModel, IItemViewModel item)
    {
        var model = item.GetModel();
        var document = documentViewModel.Document;

        var length = model.Info.Authors.Count;

        var authors = new HashSet<AuthorInfo>();

        for (int i = 0; i < length; i++)
        {
            var docAuthor = document.GetLink(model.Info.Authors, i);

            if (docAuthor != null)
            {
                authors.Add(docAuthor);
            }
        }

        Authors = authors.ToArray();

        length = model.Info.Sources.Count;

        var sources = new HashSet<SourceInfo>();

        for (int i = 0; i < length; i++)
        {
            var docSource = document.GetLink(model.Info.Sources, i);

            if (docSource != null)
            {
                sources.Add(docSource);
            }
        }

        Sources = sources.ToArray();

        if (model is Question question)
        {
            foreach (var atom in question.Scenario)
            {
                if (!atom.IsLink)
                {
                    continue;
                }

                var collection = atom.Type switch
                {
                    AtomTypes.Image => documentViewModel.Images,
                    AtomTypes.Audio => documentViewModel.Audio,
                    AtomTypes.Video => documentViewModel.Video,
                    _ => null,
                };

                if (collection == null)
                {
                    continue;
                }

                var targetCollection = atom.Type switch
                {
                    AtomTypes.Image => Images,
                    AtomTypes.Audio => Audio,
                    AtomTypes.Video => Video,
                    _ => null,
                };

                var link = atom.Text[1..];

                if (!targetCollection.ContainsKey(link))
                {
                    var preparedMedia = collection.Wrap(link);
                    targetCollection.Add(link, preparedMedia.Uri);
                }
            }
        }
    }
}
