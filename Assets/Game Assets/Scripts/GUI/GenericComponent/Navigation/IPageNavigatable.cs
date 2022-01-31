namespace Game_Assets.Scripts.GUI.GenericComponent {
	public interface IPageNavigatable {
		public void NextPage();
		public void PrevPage();
		public void SetPage(int page);
		public int page { get; }
		public int pages { get; }
	}
}