window.fetchConfigJson = async function() {
    const resp = await fetch('appsettings.json');
    if (!resp.ok) return null;
    return await resp.text();
};
