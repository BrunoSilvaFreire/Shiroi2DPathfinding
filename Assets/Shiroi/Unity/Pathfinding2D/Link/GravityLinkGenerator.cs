using System;
using System.Collections.Generic;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D.Link {
    public abstract class GravityLinkGenerator : AbstractLinkGenerator {
        public const int DefaultDivisions = 5;

        [Show]
        public float TimeIncrementation = 0.1f;

        protected GravityLinkGenerator(LinkMap linkMap) : base(linkMap) {
            this.Entity = linkMap.Entity;
            if (Entity == null) {
                throw new ArgumentException("Link map entity must not be null!");
            }
        }

        public IGroundEntity Entity {
            get;
            set;
        }

        protected abstract int SpeedIterations {
            get;
        }

        protected abstract int JumpIterations {
            get;
        }

        public override IEnumerable<ILink> Generate(Node node) {
            var list = new List<ILink>();
            for (var x = 0; x < SpeedIterations; x++) {
                var xSpeed = (float) x / SpeedIterations;
                for (var y = 0; y < JumpIterations; y++) {
                    var ySpeed = (float) y / JumpIterations;
                    var xFinal = Entity.MaxSpeed * xSpeed;
                    var yFinal = Entity.JumpHeight * ySpeed;
                    var leftVector = new Vector2(-xFinal, yFinal);
                    CheckGeneratedLink(list, GenerateLink(node, leftVector));
                    var rightVector = new Vector2(xFinal, yFinal);
                    CheckGeneratedLink(list, GenerateLink(node, rightVector));
                }
            }
            return list;
        }

        private static void CheckGeneratedLink(ICollection<ILink> list, ILink generatedLink) {
            var from = generatedLink.From;
            var to = generatedLink.To;
            if (from == null || to == null || from.Platform == to.Platform || !from.Walkable || !to.Walkable) {
                return;
            }
            list.Add(generatedLink);
        }

        protected abstract ILink GenerateLink(Node node, Vector2 speed);
    }

    public class FallLinkGenerator : GravityLinkGenerator {
        [Show]
        public int SpeedDivisions = DefaultDivisions;

        public FallLinkGenerator(LinkMap linkMap) : base(linkMap) {
        }

        protected override int SpeedIterations {
            get { return SpeedDivisions; }
        }

        protected override int JumpIterations {
            get { return 1; }
        }

        protected override ILink GenerateLink(Node node, Vector2 speed) {
            return new GravityLink(node, speed, TimeIncrementation, TileMap, Entity, LinkType.Fall);
        }
    }

    public class JumpLinkGenerator : GravityLinkGenerator {
        [Show]
        public int SpeedDivisions = DefaultDivisions;

        [Show]
        public int JumpDivisions = DefaultDivisions;

        public JumpLinkGenerator(LinkMap linkMap) : base(linkMap) {
        }

        protected override int SpeedIterations {
            get { return SpeedDivisions; }
        }

        protected override int JumpIterations {
            get { return JumpDivisions; }
        }

        protected override ILink GenerateLink(Node node, Vector2 speed) {
            return new GravityLink(node, speed, TimeIncrementation, TileMap, Entity, LinkType.Jump);
        }
    }
}