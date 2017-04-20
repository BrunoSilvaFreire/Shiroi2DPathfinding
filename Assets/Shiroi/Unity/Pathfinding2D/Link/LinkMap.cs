using System.Collections.Generic;
using System.Linq;
using Shiroi.Unity.Pathfinding2D.Util;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D.Link {
    public class LinkPoint {
        public Node Node;

        private SerializableDictionary<ILinkGenerator, List<ILink>> links =
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
        private List<ILinkGenerator> generators = new List<ILinkGenerator>();

        private readonly SerializableDictionary<Node, LinkPoint> linkPoints =
            new SerializableDictionary<Node, LinkPoint>();

        public List<ILinkGenerator> Generators {
            get { return generators; }
            set {
                if (generators.IsEmpty()) {
                    generators.AddRange(CreateDefaultGenerators());
                }
                generators = value;
            }
        }

        private IEnumerable<ILinkGenerator> CreateDefaultGenerators() {
            return new[] {
                new RunLinkGenerator(this)
            };
        }

        [Show]
        public int TotalLinks {
            get { return linkPoints.Values.Sum(point => point.Links.Count); }
        }

        [Show]
        public void Clear() {
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
            Gizmos.color = Color.green;
            foreach (var point in linkPoints.Values) {
                foreach (var link in point.Links) {
                    link.DrawGizmos();
                }
            }
        }
    }
}