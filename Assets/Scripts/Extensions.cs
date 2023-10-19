using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;


public static class TransformExtensions
{
    /// <summary> Destroys all children of the Transform </summary>
    public static void DestroyChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /// <summary> Find the rotation to look at the target Vector3 </summary>
    public static Quaternion GetLookAtRotation(this Transform self, Vector3 target)
    {
        return Quaternion.LookRotation(target - self.position);
    }

    /// <summary> Find the rotation to look away from the target Vector3 </summary>
    public static Quaternion GetLookAwayRotation(this Transform self, Vector3 target)
    {
        return Quaternion.LookRotation(self.position - target);
    }

    /// <summary> Returns the distance between two Transforms </summary>
    public static float DistanceTo(this Transform transform, Transform other) => Vector3.Distance(transform.position, other.position);

    /// <summary> Looks at a target GameObject </summary>
    public static void LookAt(this Transform transform, GameObject target) => transform.LookAt(target.transform.position);

    /// <summary> Looks at a target Vector3 </summary>
    public static void LookAt(this Transform transform, Vector3 target) => transform.LookAt(target);

    /// <summary> Looks away from a target Transform </summary>
    public static void LookAwayFrom(this Transform transform, Transform target) => transform.rotation = GetLookAwayRotation(transform, target.position);

    /// <summary> Looks away from a target GameObject </summary>
    public static void LookAwayFrom(this Transform transform, GameObject target) => transform.rotation = GetLookAwayRotation(transform, target.transform.position);

    /// <summary> Looks away from a target Vector3 </summary>
    public static void LookAwayFrom(this Transform transform, Vector3 target) => transform.rotation = GetLookAwayRotation(transform, target);
}


public static class VectorExtensions
{
    /// <summary> Returns a new Vector3 with the Y value changed to 0</summary>
    public static Vector3 Flatten(this Vector3 vector) => new Vector3(vector.x, 0, vector.z);

    /// <summary> Transforms a Vector3 into a Vector3Int</summary>
    public static Vector3Int ToVector3Int(this Vector3 vector) => new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
    
    /// <summary> Transforms a Vector3 into a Vector2</summary>
    public static Vector2 ToVector2(this Vector3 vector) => new Vector2(vector.x, vector.y);
    
    /// <summary> Transforms a Vector3 into a Vector2Int </summary>
    public static Vector2Int ToVector2Int(this Vector3 vector) => new Vector2Int((int)vector.x, (int)vector.y);
}



public static class GameObjectExtensions
{
    /// <summary> Returns the component of any type. If one doesn't already exist on the GameObject, one will be added. </summary>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }


    /// <summary> Enable the GameObject </summary>
    public static void Enable(this GameObject gameObject) => gameObject.SetActive(true);
    
    /// <summary> Disable the GameObject </summary>
    public static void Disable(this GameObject gameObject) => gameObject.SetActive(false);

    /// <summary> Returns true if the GameObject has the component of specified type. </summary>
    public static bool HasComponent<T>(this GameObject gameObject) where T : Component => gameObject.GetComponent<T>() != null;

    /// <summary> Returns the distance between two GameObjects </summary>
    public static float DistanceTo(this GameObject gameObject, GameObject other) => Vector3.Distance(gameObject.transform.position, other.transform.position);

    /// <summary> Perform an action if a component exists, skip otherwise </summary>
    public static T GetComponent<T>(this GameObject self, System.Action<T> callback) where T : Component
    {
        var component = self.GetComponent<T>();

        if (component != null)
        {
            callback.Invoke(component);
        }

        return component;
    }
}


public static class ListExtensions
{
    /// <summary> Returns a random element from the list </summary>
    public static T Random<T>(this IList<T> list) => list[UnityEngine.Random.Range(0, list.Count)];

    /// <summary> Shuffles the list </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary> Returns a random element from the list and removes it </summary>
    public static T PopRandom<T>(this IList<T> list)
    {
        int index = UnityEngine.Random.Range(0, list.Count);
        T element = list[index];
        list.RemoveAt(index);
        return element;
    }


    /// <summary> Takes an action on a specific component for every object in the array </summary>
    public static void ForEachComponent<T>(this T[] array, System.Action<T> callback) where T : Component
    {
        for (var i = 0; i < array.Length; i++)
        {
            callback.Invoke(array[i]);
        }
    }

    /// <summary> Takes an action on a specific component for every object in the list </summary>
    public static void ForEachComponent<T>(this IList<T> list, System.Action<T> callback) where T : Component
    {
        for (var i = 0; i < list.Count; i++)
        {
            callback.Invoke(list[i]);
        }
    }
}



public static class StringExtensions
{
    /// <summary> Returns a substring of the string, starting at the beginning </summary>
    public static string Trunc(this string value, int maxLength) => value.Length <= maxLength ? value : value.Substring(0, maxLength);

    /// <summary> Returns a substring of the string, starting at the end </summary>
    public static string ReverseTrunc(this string value, int maxLength) => value.Length <= maxLength ? value : value.Substring(value.Length - maxLength, maxLength);
    

    /// <summary> Returns a custom formatted ToString </summary>
    /// <example>
    ///     <code>
    ///     
    ///     public class TestHuman{
    ///         public string name;
    ///         public int age;
    ///         public TestHuman(string name, int age){
    ///            this.name = name;
    ///         this.age = age;
    ///         }
    ///     }
    ///     
    ///     TestHuman human = new TestHuman("Bob", 20);
    ///     Debug.Log(human.ToString("Human is {name} & {age} years old"));
    ///     
    ///     </code>
    /// </example>
    /// <returns> Human is Bob & 20 years old </returns>
    /// <remarks> http://www.hanselman.com/blog/CommentView.aspx?guid=fde45b51-9d12-46fd-b877-da6172fe1791 </remarks>
    /// <remarks> https://gist.github.com/omgwtfgames/f917ca28581761b8100f </remarks>
    public static string ToString(this object thisObject, string format){
        return ToString(thisObject, format, null);
    }

    public static string ToString(this object anObject, string aFormat, IFormatProvider formatProvider)
    {
        StringBuilder sb = new StringBuilder();
        Type type = anObject.GetType();
        Regex reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
        MatchCollection mc = reg.Matches(aFormat);
        int startIndex = 0;
        foreach (Match m in mc)
        {
            Group g = m.Groups[2]; //it's second in the match between { and }
            int length = g.Index - startIndex - 1;
            sb.Append(aFormat.Substring(startIndex, length));

            string toGet = string.Empty;
            string toFormat = string.Empty;
            int formatIndex = g.Value.IndexOf(":"); //formatting would be to the right of a :
            if (formatIndex == -1) //no formatting, no worries
            {
                toGet = g.Value;
            }
            else //pickup the formatting
            {
                toGet = g.Value.Substring(0, formatIndex);
                toFormat = g.Value.Substring(formatIndex + 1);
            }

            //first try properties
            PropertyInfo retrievedProperty = type.GetProperty(toGet);
            Type retrievedType = null;
            object retrievedObject = null;
            if (retrievedProperty != null)
            {
                retrievedType = retrievedProperty.PropertyType;
                retrievedObject = retrievedProperty.GetValue(anObject, null);
            }
            else //try fields
            {
                FieldInfo retrievedField = type.GetField(toGet);
                if (retrievedField != null)
                {
                    retrievedType = retrievedField.FieldType;
                    retrievedObject = retrievedField.GetValue(anObject);
                }
            }

            if (retrievedType != null) //Cool, we found something
            {
                string result = string.Empty;
                if (toFormat == string.Empty) //no format info
                {
                    result = retrievedType.InvokeMember("ToString",
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                        , null, retrievedObject, null) as string;
                }
                else //format info
                {
                    result = retrievedType.InvokeMember("ToString",
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                        , null, retrievedObject, new object[] { toFormat, formatProvider }) as string;
                }
                sb.Append(result);
            }
            else //didn't find a property with that name, so be gracious and put it back
            {
                sb.Append("{");
                sb.Append(g.Value);
                sb.Append("}");
            }
            startIndex = g.Index + g.Length + 1;
        }
        if (startIndex < aFormat.Length) //include the rest (end) of the string
        {
            sb.Append(aFormat.Substring(startIndex));
        }
        return sb.ToString();
    }
}


public static class NumericalExtensions{

    /// <summary> Returns the same number with a random sign </summary>
    public static int WithRandomSign(this int value, float negativeProbability = 0.5f) => UnityEngine.Random.value < negativeProbability ? -value : value;
    /// <summary> Returns the same number with a random sign </summary>
    public static float WithRandomSign(this float value, float negativeProbability = 0.5f) => UnityEngine.Random.value < negativeProbability ? -value : value;


    /// <summary> Round off to the nearest multiple of a number </summary>
    public static float RoundToNearest(this float value, float nearest) => Mathf.Round(value / nearest) * nearest;
    /// <summary> Round off to the nearest multiple of a number </summary>
    public static int RoundToNearest(this int value, int nearest) => Mathf.RoundToInt(value / nearest) * nearest;


    /// <summary> Returns true if the value is even </summary>
    public static bool IsEven(this int value) => value % 2 == 0;
    /// <summary> Returns true if the value is odd </summary>
    public static bool IsOdd(this int value) => value % 2 != 0;
    /// <summary> Returns true if the value is even </summary>
    public static bool IsEven(this float value) => value % 2 == 0;
    /// <summary> Returns true if the value is odd </summary>
    public static bool IsOdd(this float value) => value % 2 != 0;


    /// <summary> Returns true if the value is between the min and max values </summary>
    public static bool IsBetween(this int value, int min, int max) => value >= min && value <= max;
    /// <summary> Returns true if the value is between the min and max values </summary>
    public static bool IsBetween(this float value, float min, float max) => value >= min && value <= max;


    /// <summary> Returns the value as a percentage </summary>
    public static float ToPercentage(this float value) => value / 100f;
    /// <summary> Returns the value as a percentage </summary>
    public static float ToPercentage(this int value) => value / 100f;

    /// <summary> Returns the value as a percentage Integer </summary>
    public static int ToPercentageInt(this float value) => Mathf.RoundToInt(value * 100f);
    /// <summary> Returns the value as a percentage Integer </summary>
    public static int ToPercentageInt(this int value) => Mathf.RoundToInt(value * 100f);

    /// <summary> Transforms a Float to an Int </summary>
    public static int ToInt(this float value) => Mathf.RoundToInt(value);

    /// <summary> Transforms an Int to a Float </summary>
    public static float ToFloat(this int value) => (float)value;

}



public static class DictionaryExtensions
{
    /// <summary> Adds or updates a key/value pair in the dictionary </summary>
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }


    /// <summary> Returns the key for a given value if it is found </summary>
    public static TKey GetKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
    {
        return dictionary.FirstOrDefault(x => EqualityComparer<TValue>.Default.Equals(x.Value, value)).Key;
    }

    /// <summary> Try to remove a key by its value, return whether it succeeded or not </summary>
    public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        if (dictionary.ContainsKey(key))
        {
            return dictionary.Remove(key);
        }
        else
        {
            return false;
        }
    }


    /// <summary> Merge two dictionaries. Values from second dictionary will override the ones from the first </summary>
    public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
    {
        foreach (var item in second)
        {
            first[item.Key] = item.Value;
        }
    }

    /// <summary> Count the number of keys that satisfy a given condition </summary>
    /// <example>
    ///    <code>
    ///    Dictionary<string, int> dict = new Dictionary<string, int>();
    ///    dict.Add("Bob", 20);
    ///    dict.Add("Joe", 30);
    ///    dict.Add("Bertha", 40);
    ///    int count = dict.CountKeys(name => name.StartsWith("B"));
    ///    Debug.Log(count); // 2
    ///    </code>
    /// </example>
    public static int CountKeys<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TKey, bool> predicate)
    {
        return dictionary.Keys.Count(predicate);
    }

    /// <summary> Count the number of values that satisfy a given condition </summary>
    /// <example>
    ///   <code>
    ///   Dictionary<string, int> dict = new Dictionary<string, int>();
    ///   dict.Add("Bob", 20);
    ///   dict.Add("Joe", 30);
    ///   dict.Add("Bertha", 40);
    ///   int count = dict.CountValues(age => age >= 30);
    ///   Debug.Log(count); // 2
    ///   </code>
    /// </example>
    public static int CountValues<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TValue, bool> predicate)
    {
        return dictionary.Values.Count(predicate);
    }
}