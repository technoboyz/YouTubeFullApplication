namespace YouTubeFullApplication.Validation
{
    public static class FluentValidatorMessage
    {
        public static readonly string MaxLengthMessage = "{PropertyName} può contenere max {MaxLength} caratteri";
        public static readonly string Obbligatorio = "{PropertyName} è obbligatorio";
        public static readonly string Obbligatoria = "{PropertyName} è obbligatoria";
        public static readonly string NonValido = "{PropertyName} non valido";
        public static readonly string NonValida = "{PropertyName} non valida";
    }
}
