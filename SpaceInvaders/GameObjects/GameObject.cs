namespace SpaceInvaders
{
	
	abstract class GameObject
	{
		public GameObjectLocation GameObjectLocation { get; set; }

		public char Figure { get; set; }
	}
}