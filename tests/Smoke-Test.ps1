param([string]$BaseUrl = 'http://localhost:5180')
$ErrorActionPreference = 'Stop'
$created = @{}
function Request($method, $path, $body, $expected) {
    $params = @{Method=$method; Uri="$BaseUrl/api/$path"; SkipHttpErrorCheck=$true}
    if ($null -ne $body) { $params.ContentType='application/json'; $params.Body=($body | ConvertTo-Json) }
    $response = Invoke-WebRequest @params
    if ([int]$response.StatusCode -ne $expected) { throw "$method $path expected $expected, got $($response.StatusCode): $($response.Content)" }
    if ($response.Content) { return ($response.Content | ConvertFrom-Json) }
}
try {
    $tag = [guid]::NewGuid().ToString('N').Substring(0,8)
    $created.cursos = (Request POST cursos @{nombre="Prueba $tag"} 201).cursoId
    $created.estudiantes = (Request POST estudiantes @{nombres='Prueba';apellidos=$tag;direccion='Cuilapa'} 201).estudianteId
    $created.docentes = (Request POST docentes @{nombres='Docente';apellidos=$tag} 201).docenteId
    $body = @{estudianteId=$created.estudiantes;cursoId=$created.cursos;docenteId=$created.docentes}
    $created.asignaciones = (Request POST asignaciones $body 201).asignacionId
    foreach ($type in @('cursos','estudiantes','docentes','asignaciones')) {
        $null = Request GET $type $null 200
        $null = Request GET "$type/$($created[$type])" $null 200
    }
    $null = Request PUT "cursos/$($created.cursos)" @{nombre="Actualizado $tag"} 204
    $course = Request GET "cursos/$($created.cursos)" $null 200
    if ($course.nombre -ne "Actualizado $tag") { throw 'La actualización no persistió' }
    $null = Request PUT "estudiantes/$($created.estudiantes)" @{nombres='Editado';apellidos=$tag;direccion='Guatemala'} 204
    $null = Request PUT "docentes/$($created.docentes)" @{nombres='Editado';apellidos=$tag} 204
    $null = Request PUT "asignaciones/$($created.asignaciones)" $body 204
    foreach ($type in @('cursos','estudiantes','docentes')) { $null = Request DELETE "$type/$($created[$type])" $null 409 }
    $null = Request POST cursos @{nombre='   '} 400
    $null = Request POST cursos @{nombre=('x'*151)} 400
    $null = Request POST asignaciones @{estudianteId=2147483647;cursoId=$created.cursos;docenteId=$created.docentes} 400
    $null = Request POST asignaciones @{estudianteId=0;cursoId=0;docenteId=0} 400
    $null = Request GET cursos/2147483647 $null 404
    Write-Output 'PASS: CRUD de cuatro entidades, persistencia, validación, referencias y protección de borrado.'
} finally {
    foreach ($type in @('asignaciones','cursos','estudiantes','docentes')) {
        if ($created.ContainsKey($type)) { $null = Request DELETE "$type/$($created[$type])" $null 204 }
    }
}
