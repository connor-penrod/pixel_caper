using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class PaintingButton : ButtonActivated
    {
        public Sprite realPainting;
        public Sprite fakePainting;
        public bool stolenPainting = false;

        [HideInInspector]
        public Vector3 oldColliderSize, oldScale, newScale, newColliderSize;

        public PaintingButton(Sprite realPainting,Sprite fakePainting)
        {
            this.realPainting = realPainting;
            this.fakePainting = fakePainting;
        }


        override public void TriggerButtonAction()
        {
            if (stolenPainting==false)
            {
                this.GetComponent<SpriteRenderer>().sprite = fakePainting;//Changes Painting on Wall
                oldColliderSize = this.GetComponent<BoxCollider2D>().size;
                oldScale = this.transform.localScale;
                newScale = new Vector3(this.transform.localScale.x * realPainting.bounds.size.x / fakePainting.bounds.size.x, this.transform.localScale.y * realPainting.bounds.size.y / fakePainting.bounds.size.y, 1);
                this.transform.localScale = newScale;
                newColliderSize = new Vector3(oldColliderSize.x * oldScale.x / newScale.x, oldColliderSize.y * oldScale.y / newScale.y);
                this.GetComponent<BoxCollider2D>().size = newColliderSize;
                stolenPainting = true;//Sets Stolen Flag
                this.GetComponent<BoxCollider2D>().enabled = false;
                this.transform.Rotate(new Vector3(0, 0, -20));//Skews Fake Painting
            }//If Not Stolen, steal painting
        }
    }//Controls Painting Activations
}
