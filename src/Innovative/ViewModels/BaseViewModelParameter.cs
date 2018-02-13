namespace PSY.Innovative.ViewModels
{
    public class BaseViewModelParameter
    {
        //ToDo refator IsModal in baseviemodal based on ShowModalViewModel ...etc and call close or cancel.
        public BaseViewModelParameter(bool isModal)
        {
            IsModal = isModal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value><c>true</c> if is modal; otherwise, <c>false</c>.</value>
        public bool IsModal { get; private set; }
    }
}