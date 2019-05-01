using System.Collections.Generic;
using UnityEngine;

namespace AIPlanner
{
    public class HTNViewer : MonoBehaviour
    {
        IHTNView view;

        public PlanState planState;
        public string activeTask;
        public ActionState actionState;
        public List<string> plan = new List<string>();


        void OnEnable()
        {
            view = GetComponent<IHTNView>();
            if (view == null) enabled = false;
        }

        void Update()
        {
            plan.Clear();
            // if (view.Plan != null)
            //     foreach (var i in view.Plan)
            //         plan.Add(i.name);
            planState = view.PlanRunner.PlanState;
            if (view.PlanRunner.ActiveTask != null)
                activeTask = view.PlanRunner.ActiveTask.name;
            actionState = view.PlanRunner.ActiveState;
        }
    }
}