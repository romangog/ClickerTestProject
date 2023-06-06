using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlUpButtonView : EntityAddition<LvlUpBusinessButtonComponent>
{
    public void SetBusinessEntity(int businessEntity)
    {
        _component.BusinessEntity = businessEntity;
    }
}


