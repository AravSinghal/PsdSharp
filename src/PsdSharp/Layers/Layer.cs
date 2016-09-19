﻿// Copyright (c) 2016 Arav Singhal
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of PsdSharp and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation but with attribution the rights
// to use, copy, modify, merge and/or publish copies of the Software but NOT distribute, sublicense 
// or sell copies of the Software without prior permission and attribution of the author(s), and to 
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software. 
// Furthermore, the above copyright notice shall not be removed from this file.
// 
// Include the MIT License NO WARRANTY clause here.

using System.Collections.Generic;
using System.IO;
using PsdSharp.Internal;
using PsdSharp.IO;

namespace PsdSharp.Layers
{
    public class Layer
    {
        public List<Channel> Channels { get; set; }

        public Rectangle Rect { get; set; }

        public string BlendMode { get; set; }

        public byte Opacity { get; set; }

        internal static void LoadIntoDocument(PsdDocument psdDocument, BigEndianBinaryReader reader)
        {
            Layer layer = Load(reader);
        }

        internal static Layer Load(BigEndianBinaryReader reader)
        {
            Layer layer = new Layer();

            layer.Rect = new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(),
                reader.ReadInt32());

            // Next two bytes contain number of channels
            layer.Channels = new List<Channel>(reader.ReadInt16());

            for (int i = 0; i < layer.Channels.Count; i++)
                layer.Channels[i] = Channel.Load(reader);

            string blendModeSignature = new string(reader.ReadChars(4));

            if (!blendModeSignature.Equals(Constants.ImageResourceSignature))
                throw new IOException("Invalid blend mode.");

            // TODO: Use enum instead
            layer.BlendMode = new string(reader.ReadChars(4));

            layer.Opacity = reader.ReadByte();

            

            return layer;
        }
    }
}