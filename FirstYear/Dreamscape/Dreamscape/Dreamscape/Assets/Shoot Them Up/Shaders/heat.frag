uniform float time; // temps en secondes depuis le début de l'exécution
uniform sampler2D texture;

void main()
{
    // Récupérer la couleur du pixel de la texture
    vec4 color = texture2D(texture, gl_TexCoord[0].xy);

    // Calcul de la position du pixel dans la texture (entre 0 et 1)
    vec2 texCoord = gl_TexCoord[0].xy;

    // Calcul de la nouvelle position en utilisant une onde sinusoïdale
    float distortion = sin(texCoord.y * 20.0 + time * 5.0) * 0.01;
    vec2 waveTexCoord = vec2(texCoord.x, texCoord.y + distortion);

    // Récupération de la couleur modifiée en utilisant la nouvelle position
    vec4 wave_color = texture2D(texture, waveTexCoord);

    // Mélange des couleurs manuellement
    vec4 final_color = vec4(0.5, 0.5, 0.5, 0.5); // Couleur initiale
    final_color += color * 0.5; // Ajout de la couleur originale
    final_color += wave_color * 0.5; // Ajout de la couleur modifiée

    // Définition de la couleur finale du pixel
    gl_FragColor = final_color;
}
