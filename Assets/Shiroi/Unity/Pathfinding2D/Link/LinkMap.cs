using System;
using System.Collections.Generic;
using System.Linq;
using Shiroi.Unity.Pathfinding2D.Util;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D.Link {
    public class LinkPoint {
        public Node Node;

        private readonly SerializableDictionary<ILinkGenerator, List<ILink>> links =
            new SerializableDictionary<ILinkGenerator, List<ILink>>();

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
    }

    public class LinkMap : BaseBehaviour {
        public IGroundEntity Entity;
        public TileMap TileMap;
        public Color RunLinkColor = ColorUtil.FromHex("FFC107");
        public Color JumpLinkColor = ColorUtil.FromHex("6D6A5E");
        public Color FallLinkColor = ColorUtil.FromHex("5E676D");

        [SerializeField]
        private List<ILinkGenerator> generators = new List<ILinkGenerator>();

        [SerializeField, Hide]
        private readonly SerializableDictionary<Node, LinkPoint> linkPoints =
            new SerializableDictionary<Node, LinkPoint>();

        public List<ILinkGenerator> Generators {
            get {
                if (generators.IsEmpty()) {
                    generators.AddRange(CreateDefaultGenerators());
                }
                return generators;
            }
            set { generators = value; }
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

        private LinkPoint GetLinkPoint(Node node) {
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
                case LinkType.Run:
                    return RunLinkColor;
                case LinkType.Fall:
                    return FallLinkColor;
                case LinkType.Jump:
                    return JumpLinkColor;
                default:
                    throw new ArgumentOutOfRangeException(linkType.ToString());
            }
        }
    }
}