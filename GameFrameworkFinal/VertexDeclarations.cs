using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{

/// <summary>
/// A vertex structure that contains a position and also two texture coordinates
/// </summary>
public struct VertexPositionDualTexture : IVertexType
{
    //-------------------------------------------------------------------------------------
    // Public fields

    /// <summary>
    ///  The vertex position
    /// </summary>
    public Vector3 Position;
    /// <summary>
    /// The texture coordinate for the first texture
    /// </summary>
    public Vector2 TexCoord0;
    /// <summary>
    /// The texture coordinate for the second texture
    /// </summary>
    public Vector2 TexCoord1;

    //-------------------------------------------------------------------------------------
    // Constructors

    /// <summary>
    /// Allow all property values to be provided to the constructor
    /// </summary>
    public VertexPositionDualTexture(Vector3 position, Vector2 texCoord0, Vector2 texCoord1)
    {
        // Set properties
        this.Position = position;
        this.TexCoord0 = texCoord0;
        this.TexCoord1 = texCoord1;
    }

    //-------------------------------------------------------------------------------------
    // Vertex declaration properties

    /// <summary>
    /// Define the content and layout of the vertex declaration
    /// </summary>
    public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
    (
        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
        new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
        new VertexElement(20, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1)
    );

    /// <summary>
    /// Return the vertex declaration
    /// </summary>
    VertexDeclaration IVertexType.VertexDeclaration
    {
        get
        {
            return VertexDeclaration;
        }
    }
};

}
