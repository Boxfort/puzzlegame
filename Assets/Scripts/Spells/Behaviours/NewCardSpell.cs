using System;

public class NewCardSpell : ISpellBehaviour
{
	public void Execute()
	{
		CardManager.GetInstance().NewCard();
	}

    public void Cancel() { /* Do nothing */ }
}

