namespace SistemaConvenios.Models;

public sealed class TipoConvenio
{
    private TipoConvenio()
    {
    }

    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    public ICollection<Convenio> Convenios { get; private set; } = new List<Convenio>();
}

public sealed class Facultad
{
    private Facultad()
    {
    }

    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string? Siglas { get; private set; }
    public bool Activo { get; private set; } = true;
    public ICollection<Carrera> Carreras { get; private set; } = new List<Carrera>();
    public ICollection<ConvenioFacultad> ConvenioFacultades { get; private set; } = new List<ConvenioFacultad>();
}

public sealed class Carrera
{
    private Carrera()
    {
    }

    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string? Siglas { get; private set; }
    public int FacultadId { get; private set; }
    public Facultad? Facultad { get; private set; }
    public bool Activo { get; private set; } = true;
    public ICollection<ConvenioCarrera> ConvenioCarreras { get; private set; } = new List<ConvenioCarrera>();
}

public sealed class AreaPromotora
{
    private AreaPromotora()
    {
    }

    public int Id { get; private set; }
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public bool Activo { get; private set; } = true;
    public ICollection<Convenio> Convenios { get; private set; } = new List<Convenio>();
}
