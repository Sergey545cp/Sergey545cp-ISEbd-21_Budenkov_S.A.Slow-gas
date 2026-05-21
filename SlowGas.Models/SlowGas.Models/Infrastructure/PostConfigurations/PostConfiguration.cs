namespace SlowGas.Models.Infrastructure.PostConfigurations
{
    public class PostConfiguration
    {
        public virtual string Type => nameof(PostConfiguration);
        public double Rate { get; set; }
    }
}