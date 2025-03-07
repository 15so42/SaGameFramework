using System;
using System.Collections.Generic;
using System.Linq;

public enum ModifierType
{
    Flat,    // 平坦加成
    Percent  // 百分比加成
}

public class Modifier<T>
{
    public string Source { get; } // 修饰器来源的唯一标识
    public T Value { get; }       // 修饰值
    public ModifierType Type { get; } // 修饰器类型

    public Modifier(string source, T value, ModifierType type)
    {
        Source = source;
        Value = value;
        Type = type;
    }
}

public class Property<T> where T : struct, IConvertible
{
    private T baseValue; // 基础值
    private Dictionary<string, Modifier<T>> modifiers = new Dictionary<string, Modifier<T>>();

    public Property(T baseValue)
    {
        this.baseValue = baseValue;
    }


    public T GetBaseValue()
    {
        return baseValue;
    }

    /// 设置基础值
    public void SetBaseValue(T value)
    {
        baseValue = value;
    }

    /// 更新或添加修饰器
    public void UpdateModifier(Modifier<T> modifier)
    {
        if (modifiers.ContainsKey(modifier.Source))
        {
            // 如果已存在相同来源的修饰器，更新值
            modifiers[modifier.Source] = modifier;
        }
        else
        {
            // 添加新的修饰器
            modifiers.Add(modifier.Source, modifier);
        }
    }

    public void RemoveModifier(string key)
    {
        if (modifiers.ContainsKey(key))
        {
            modifiers.Remove(key);
        }
        
    }
    
    

    /// 计算最终值
    public T CalculateFinalValue()
    {
        double result = Convert.ToDouble(baseValue);

        // 累加平坦修正
        foreach (var modifier in modifiers.Values.Where(m => m.Type == ModifierType.Flat))
        {
            result += Convert.ToDouble(modifier.Value);
        }

        // 应用百分比修正
        foreach (var modifier in modifiers.Values.Where(m => m.Type == ModifierType.Percent))
        {
            result *= (1 + Convert.ToDouble(modifier.Value));
        }

        return (T)Convert.ChangeType(result, typeof(T));
    }
}