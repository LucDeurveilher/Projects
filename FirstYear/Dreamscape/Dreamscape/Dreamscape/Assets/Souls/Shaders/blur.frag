#version 120

uniform sampler2D texture;
uniform vec2 screenSize;
uniform float intensity;

void main()
{
    vec2 texcoord = gl_TexCoord[0].xy;

    vec4 sum = vec4(0.0);
    vec2 sizeFactor = 1.0 / screenSize.xy;
    float weightSum = 0.0;

    for (float x = -intensity; x <= intensity; x += 1.0)
    {
        for (float y = -intensity; y <= intensity; y += 1.0)
        {
            vec2 offset = vec2(x, y) * sizeFactor;
            float weight = exp(-(x * x + y * y) / (2.0 * intensity * intensity)) / (2.0 * 3.14159 * intensity * intensity);
            sum += texture2D(texture, texcoord + offset) * weight;
            weightSum += weight;
        }
    }

    gl_FragColor = sum / weightSum;
}