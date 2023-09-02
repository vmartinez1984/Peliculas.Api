using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Peliculas.Api.Helpers
{
    public class TypeBinder <T>: IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nombrePropiedad = bindingContext.ModelName;
            var valor = bindingContext.ValueProvider.GetValue(nombrePropiedad);
            if(valor == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                var valorDeserealizado = JsonConvert.DeserializeObject<T>(valor.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(valorDeserealizado);
            }
            catch (Exception)
            {

                bindingContext.ModelState.TryAddModelError(nombrePropiedad, "El valor no es del tipo adecuado");
            }

            return Task.CompletedTask;
        }
    }
}
