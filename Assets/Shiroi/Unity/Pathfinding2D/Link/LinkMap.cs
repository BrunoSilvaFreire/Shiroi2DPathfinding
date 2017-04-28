using System;
using System.Collections.Generic;
using System.Linq;
using Shiroi.Unity.Pathfinding2D.Util;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;
using Vexe.Runtime.Types.Others;

namespace Shiroi.Unity.Pathfinding2D.Link {
    [Serializable]
    public class LinkPoint {
        public Node Node;

        private readonly LinkListDictionary links = new LinkListDictionary();

        public List<ILink> GetLinks(ILinkGenerator generator) {
            return links.GetOrPut(generator, () => new List<ILink>());
        }

        public List<ILink> Links {
            get {
                var list = new List<ILink>();
                foreach (var linkList in links.Values) {
                    list.AddRange(linkList);
                }
                return list;
            }
        }

        public LinkPoint(Node node) {
            Node = node;
        }

        public void GenerateLinks(ILinkGenerator generator) {
            var generatedLinks = generator.Generate(Node);
            GetLinks(generator).AddRange(generatedLinks);
        }

        public ILink GetLink(Node to) {
            return Links.First(link => link.To == to);
        }
    }

    [Serializable]
    class LinkListDictionary : SerializableDictionary<ILinkGenerator, List<ILink>> {
    }

    public class LinkMap : BaseBehaviour {
        public IGroundEntity Entity;
        public TileMap TileMap;
        public Color RunLinkColor = ColorUtil.FromHex("FFC107");
        public Color JumpLinkColor = ColorUtil.FromHex("ff1744", 15);
        public Color FallLinkColor = ColorUtil.FromHex("76FF03", 35);

        [SerializeField, Hide]
        private readonly LinkPointDictionary linkPoints = new LinkPointDictionary();

        public LinkMap() {
            Generators = CreateGenerators();
        }

        private List<ILinkGenerator> CreateGenerators() {
            var gen = new List<ILinkGenerator>();
            if (Entity != null) {
                gen.AddRange(CreateDefaultGenerators());
            } else {
                Debug.LogWarning("Could not create default generators because the LinkMap's Entity is null");
            }
            return gen;
        }

        [Show]
        public void RecreateGenerators() {
            Generators = CreateGenerators();
        }

        [Show]
        public List<ILinkGenerator> Generators {
            get;
            set;
        }

        private IEnumerable<ILinkGenerator> CreateDefaultGenerators() {
            return new ILinkGenerator[] {
                new RunLinkGenerator(this), new FallLinkGenerator(this), new JumpLinkGenerator(this)
            };
        }

        [Show]
        public int TotalLinks {
            get { return linkPoints.Values.Sum(point => point.Links.Count); }
        }

        [Show]
        public void Clear() {
            linkPoints.Clear();
        }

        [Show]
        public void GenerateLinks() {
            Clear();
            foreach (var node in TileMap) {
                foreach (var generator in Generators) {
                    GetLinkPoint(node).GenerateLinks(generator);
                }
            }
            Debug.Log(string.Format("Calculated a total of {0} links.", TotalLinks));
        }

        public LinkPoint GetLinkPoint(Node node) {
            return linkPoints.GetOrPut(node, () => new LinkPoint(node));
        }

        private void OnDrawGizmosSelected() {
            foreach (var point in linkPoints.Values) {
                foreach (var link in point.Links) {
                    Gizmos.color = GetColor(link.Type);
                    link.DrawGizmos();
                }
            }
        }

        private Color GetColor(LinkType linkType) {
            switch (linkType) {
                case LinkType.Run: return RunLinkColor;
                case LinkType.Fall: return FallLinkColor;
                case LinkType.Jump: return JumpLinkColor;
                default: throw new ArgumentOutOfRangeException(linkType.ToString());
            }
        }

        public ILink GetLink(Node from, Node to) {
            return GetLinkPoint(from).GetLink(to);
        }
    }
    [Serializable]
    public class LinkPointDictionary : SerializableDictionary<Node, LinkPoint> {
    }
}