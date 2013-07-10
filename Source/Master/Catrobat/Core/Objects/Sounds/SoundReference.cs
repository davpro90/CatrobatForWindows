﻿using System.ComponentModel;
using System.Xml.Linq;
using Catrobat.Core.Misc.Helpers;

namespace Catrobat.Core.Objects.Sounds
{
    public class SoundReference : DataObject
    {
        private readonly Sprite _sprite;

        private string _reference;
        public string Reference
        {
            get { return _reference; }
            set
            {
                if (_reference == value)
                {
                    return;
                }

                _reference = value;
                RaisePropertyChanged();
            }
        }

        private Sound _sound;
        public Sound Sound
        {
            get { return _sound; }
            set
            {
                if (_sound == value)
                {
                    return;
                }

                _sound = value;
                RaisePropertyChanged();
            }
        }

        public SoundReference(Sprite parent)
        {
            _sprite = parent;
        }

        public SoundReference(XElement xElement, Sprite parent)
        {
            _sprite = parent;
            LoadFromXML(xElement);
        }

        internal override void LoadFromXML(XElement xRoot)
        {
            _reference = xRoot.Attribute("reference").Value;
            Sound = XPathHelper.GetElement(_reference, _sprite) as Sound;
        }

        internal override XElement CreateXML()
        {
            var xRoot = new XElement("sound");

            xRoot.Add(new XAttribute("reference", XPathHelper.GetReference(Sound, _sprite)));

            return xRoot;
        }

        public DataObject Copy(Sprite parent)
        {
            var newSoundInfoRef = new SoundReference(parent);
            newSoundInfoRef._reference = _reference;
            newSoundInfoRef.Sound = XPathHelper.GetElement(_reference, parent) as Sound;

            return newSoundInfoRef;
        }
    }
}