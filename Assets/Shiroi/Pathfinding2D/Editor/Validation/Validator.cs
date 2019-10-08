using System;
using System.Collections.Generic;
using Shiroi.Pathfinding2D.Runtime;
using UnityEditor;

namespace Shiroi.Pathfinding2D.Editor.Validation {
    public delegate String Validation<T>(T value);

    public class Validator<T> {
        private readonly Validation<T> validation;

        public Validator(Validation<T> validation) {
            this.validation = validation;
        }

        public string Validate(T value) {
            return validation(value);
        }

        public bool Check(T value) {
            return validation(value) == null;
        }

        public static implicit operator Validator<T>(Validation<T> v) {
            return new Validator<T>(v);
        }
    }

    public static class Validators {
        public static void ValidateInEditor<T>(T obj, params Validator<T>[] validators) {
            var errs = new List<string>();
            foreach (var validator in validators) {
                var err = validator.Validate(obj);
                if (err != null) {
                    errs.Add(err);
                }
            }

            if (errs.Count > 0) {
                using (new EditorGUILayout.VerticalScope()) {
                    foreach (var err in errs) {
                        EditorGUILayout.HelpBox(err, MessageType.Error, true);
                    }
                }
            }
        }
    }
    public static class Validators<G> {
        public static Validator<NavMesh2D<G>>
            NodesMatchGeometry = (Validation<NavMesh2D<G>>) ValidateNodesMatchGeometry;
        
        public static string ValidateNodesMatchGeometry(NavMesh2D<G> navMesh) {
            var nodes = navMesh.Nodes;
            if (nodes != null && nodes.Length != navMesh.Area) {
                return $"Number of nodes doesn't match number of tiles in navmesh. ({navMesh.Area} vs {nodes.Length})";
            }

            return null;
        }


        public static Validator<NavMesh2D<G>> HasNodes = (Validation<NavMesh2D<G>>) ValidateHasNodes;
        public static string ValidateHasNodes(NavMesh2D<G> map) {
            return map.Nodes == null
                ? "NavMesh doesn't have any nodes generated! Please press \"Generate Nodes\" when you are ready!"
                : null;
        }

    }

    public static class Validators<L, G> {
        public static string ValidateLinksMatchGeometry(LinkMap2D<L, G> map) {
            var n = map.NavMesh;
            if (n == null) {
                return null;
            }

            var l = map.nodes;

            if (l != null && n.Area != l.Length) {
                return $"Number of links doesn't match number of nodes in navmesh. ({n.Area} vs {l.Length})";
            }

            return null;
        }


        public static string ValidateHasNavMesh(LinkMap2D<L, G> map) {
            return map.NavMesh == null ? "Missing navmesh! You can't generate links without a navmesh bound.'" : null;
        }

        public static Validator<LinkMap2D<L, G>> HasNavMesh = (Validation<LinkMap2D<L, G>>) ValidateHasNavMesh;

        public static Validator<LinkMap2D<L, G>> LinksMatchGeometry =
            (Validation<LinkMap2D<L, G>>) ValidateLinksMatchGeometry;

        

     
    }
}