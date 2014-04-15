using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameObjectList : GameObject
{
    protected List<GameObject> gameObjects;

    public GameObjectList(int layer = 0, string id = "") : base(layer, id)
    {
        gameObjects = new List<GameObject>();
    }

    public void Add(GameObject obj)
    {
        obj.Parent = this;
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i].Layer > obj.Layer)
            {
                gameObjects.Insert(i, obj);
                return;
            }
        }
        gameObjects.Add(obj);
    }

    public void Remove(GameObject obj)
    {
        gameObjects.Remove(obj);
        obj.Parent = null;
    }

    public GameObject Find(string id)
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj.ID == id)
                return obj;
            if (obj is GameObjectList)
            {
                GameObjectList objlist = obj as GameObjectList;//? только созданный элемент 
                GameObject subobj = objlist.Find(id);
                if (subobj != null)
                    return subobj;
            }
        }
        return null;
    }

    public List<GameObject> Objects
    {
        get { return gameObjects; }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        for (int i = gameObjects.Count - 1; i >= 0; i--)
            gameObjects[i].HandleInput(inputHelper);
    }

    public override void Update(GameTime gameTime)
    {
        foreach (GameObject obj in gameObjects)
            obj.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible)
            return;
        List<GameObject>.Enumerator e = gameObjects.GetEnumerator();        
        while (e.MoveNext())
            e.Current.Draw(gameTime, spriteBatch);
    }

    public override void Reset()
    {
        base.Reset();
        foreach (GameObject obj in gameObjects)
            obj.Reset();
    }
}
