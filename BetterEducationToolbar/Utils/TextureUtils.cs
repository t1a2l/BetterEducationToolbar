using ColossalFramework.UI;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using static ColossalFramework.Plugins.PluginManager;
using ColossalFramework.Plugins;
using System.Collections.Generic;

namespace BetterEducationToolbar
{
	internal static class TextureUtils
	{
        internal static Dictionary<string, UITextureAtlas> m_atlasStore = new Dictionary<string, UITextureAtlas>();
        static string PATH => "BetterEducationToolbar.BetterEducationToolbar.Utils.Atlas.";
        static string ModPath => GetPlugin().modPath;
        public static string FILE_PATH = ModPath;
        public static bool EmbededResources = true;
       
        public static UITextureAtlas GetAtlas(string atlasName)
        {
            UITextureAtlas returnAtlas = null;

            if (m_atlasStore.ContainsKey(atlasName))
            {
                returnAtlas = m_atlasStore[atlasName];
            }

            return returnAtlas;
        }

        static PluginManager man => PluginManager.instance;

        public static void FixTransparency(Texture2D texture)
        {
            Color32[] pixels = texture.GetPixels32();
            int w = texture.width;
            int h = texture.height;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int idx = y * w + x;
                    Color32 pixel = pixels[idx];
                    if (pixel.a == 0)
                    {
                        bool done = false;
                        if (!done && x > 0) done = TryAdjacent(ref pixel, pixels[idx - 1]);        // Left   pixel
                        if (!done && x < w - 1) done = TryAdjacent(ref pixel, pixels[idx + 1]);        // Right  pixel
                        if (!done && y > 0) done = TryAdjacent(ref pixel, pixels[idx - w]);        // Top    pixel
                        if (!done && y < h - 1) done = TryAdjacent(ref pixel, pixels[idx + w]);        // Bottom pixel
                        pixels[idx] = pixel;
                    }
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply();
        }

        private static bool TryAdjacent(ref Color32 pixel, Color32 adjacent)
        {
            if (adjacent.a == 0) return false;

            pixel.r = adjacent.r;
            pixel.g = adjacent.g;
            pixel.b = adjacent.b;
            return true;
        }

        public static bool InitialiseAtlas(string atlasName)
        {
            bool createdAtlas = false;
            Shader shader = Shader.Find("UI/Default UI Shader");

            if (shader != null)
            {
                Texture2D spriteTexture = GetTextureFromAssemblyManifest("EducationAtlas.png");
                FixTransparency(spriteTexture);

                Material atlasMaterial = new Material(shader)
                {
                    mainTexture = spriteTexture
                };

                UITextureAtlas atlas = ScriptableObject.CreateInstance<UITextureAtlas>();
                atlas.name = atlasName;
                atlas.material = atlasMaterial;

                m_atlasStore.Add(atlasName, atlas);

                createdAtlas = true;
            }
            else
            {
                Debug.LogError("SpriteUtilities: Couldn't find the default UI Shader!");
            }
            return createdAtlas;
        }



        /// Creates a new sprite using the size of the image inside the atlas.
        /// </summary>
        /// <param name="dimensions">The location and size of the sprite within the atlas (in pixels).</param>
        /// <param name="spriteName">The name of the sprite to create</param>
        /// <param name="atlasName">The name of the atlas to add the sprite to.</param>
        /// <returns></returns>
        public static bool AddSpriteToAtlas(Rect dimensions, string spriteName, string atlasName)
        {
            bool returnValue = false;

            if (m_atlasStore.ContainsKey(atlasName))
            {
                UITextureAtlas foundAtlas = m_atlasStore[atlasName];
                Texture2D atlasTexture = foundAtlas.texture;
                Vector2 atlasSize = new Vector2(atlasTexture.width, atlasTexture.height);
                Rect relativeLocation = new Rect(new Vector2(dimensions.position.x / atlasSize.x, dimensions.position.y / atlasSize.y), new Vector2(dimensions.width / atlasSize.x, dimensions.height / atlasSize.y));
                Texture2D spriteTexture = new Texture2D((int)Math.Round(dimensions.width), (int)Math.Round(dimensions.height));

                spriteTexture.SetPixels(atlasTexture.GetPixels((int)dimensions.position.x, (int)dimensions.position.y, (int)dimensions.width, (int)dimensions.height));

                UITextureAtlas.SpriteInfo createdSprite = new UITextureAtlas.SpriteInfo()
                {
                    name = spriteName,
                    region = relativeLocation,
                    texture = spriteTexture
                };

                foundAtlas.AddSprite(createdSprite);
                returnValue = true;
            }

            return returnValue;
        }

        public static Texture2D GetTextureFromAssemblyManifest(string file) {
            using (Stream stream = GetManifestResourceStream(file))
                return GetTextureFromStream(stream);
        }

        public static Stream GetManifestResourceStream(string file) {
            try {
                var d = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                string path = string.Concat(PATH, file);
                return Assembly.GetExecutingAssembly().GetManifestResourceStream(path)
                    ?? throw new Exception(path + " not found");
            } catch (Exception ex) {
                LogHelper.Error(ex.ToString());
                throw ex;
            }
        }

        public static Texture2D GetTextureFromStream(Stream stream) {
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            texture2D.filterMode = FilterMode.Bilinear;
            texture2D.LoadImage(array);
            texture2D.wrapMode = TextureWrapMode.Clamp; // for cursor.
            texture2D.Apply();
            return texture2D;
        }

        public static PluginInfo GetPlugin(Assembly assembly = null) {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();
            foreach (PluginInfo current in man.GetPluginsInfo()) {
                if (current.ContainsAssembly(assembly))
                    return current;
            }
            return null;
        }
	}
}
