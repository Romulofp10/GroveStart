namespace GroveStart.Exceptions
{
    /// <summary>
    /// Lançada quando se tenta cadastrar (ou, no futuro, alterar) para um e-mail já em uso.
    /// </summary>
    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException(string email)
            : base($"Já existe um usuário cadastrado com o e-mail informado.")
        {
            Email = email;
        }

        public string Email { get; }
    }
}
