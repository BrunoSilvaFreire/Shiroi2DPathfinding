/*using System;
using System.Collections.Generic;
using Datenshi.AI.Pathfinding.Navmesh;
using Datenshi.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.AI.Pathfinding.Navigator {
    

    public abstract class Navigator : MonoBehaviour {
        public Navmesh2D navmesh;

        [ShowInInspector]
        private NavigationTrip trip;

        private void Awake() {
            navmesh = FindObjectOfType<Navmesh2D>();
        }

        [ShowInInspector]
        public void TravelTo(uint target) {
            trip = new NavigationTrip(target);
            trip.ReloadPath(navmesh.IndexOf(Owner.transform), navmesh, this);
        }

        private void Update() {
            if (trip != null && Owner != null) {
                Navigate(Owner, trip);
            }
        }

        protected abstract void Navigate(Entity owner, NavigationTrip trip);


        public abstract bool CanTransverse(ILink link);
    }
}*/