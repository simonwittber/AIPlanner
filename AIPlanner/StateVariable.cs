using System;
using System.Collections.Generic;
using LiteSpace;
using Surrogates;
using UnityEngine;

namespace AIPlanner
{
    [System.Serializable]
    public class StateVariable
    {
        public float value;
        public int index;
        ISurrogateProperty<float> surrogate;

        public static Condition operator <=(StateVariable x, int y) => new Condition() { variable = x, fn = (z) => z.value <= y };
        public static Condition operator >=(StateVariable x, int y) => new Condition() { variable = x, fn = (z) => z.value >= y };
        public static Condition operator <(StateVariable x, int y) => new Condition() { variable = x, fn = (z) => z.value < y };
        public static Condition operator >(StateVariable x, int y) => new Condition() { variable = x, fn = (z) => z.value > y };
        public static Condition operator ==(StateVariable x, int y) => new Condition() { variable = x, fn = (z) => z.value == y };
        public static Condition operator !=(StateVariable x, int y) => new Condition() { variable = x, fn = (z) => z.value != y };

        public static implicit operator float(StateVariable d) => d.value;

        public void ConnectTo(Component component, string fieldName)
        {
            var isProperty = component.GetType().GetProperty(fieldName) != null;
            if (isProperty)
                surrogate = SurrogateRegister.GetSurrogateProperty<float>(component, fieldName);
            else
                surrogate = SurrogateRegister.GetSurrogateField<float>(component, fieldName);
        }

        public void Sync()
        {
            if (surrogate != null)
                value = surrogate.Get();
        }

        public static Effect operator -(StateVariable x, int y) => new Effect() { variable = x, fn = (z) => z.value -= y };
        public static Effect operator +(StateVariable x, int y) => new Effect() { variable = x, fn = (z) => z.value += y };
        public static Effect operator *(StateVariable x, int y) => new Effect() { variable = x, fn = (z) => z.value *= y };

        public Effect Set(float v) => new Effect() { variable = this, fn = (z) => z.value = v };


        public static Effect operator /(StateVariable x, int y) => new Effect() { variable = x, fn = (z) => z.value /= y };

        public void Inc(int v) => this.value += v;
        public void Dec(int v) => this.value -= v;
        public void Mul(int v) => this.value *= v;
        public void Div(int v) => this.value /= v;
    }


}